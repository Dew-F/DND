using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] public Color _standartColor;
    [SerializeField] private Color _hoverColor;
    [SerializeField] public bool _canStep;
    [SerializeField] public bool _cellTaken;
    [SerializeField] public GameObject _prefabCharacter;
    [SerializeField] public Vector2Int _cellPosition;

    [SerializeField] private MeshRenderer _meshRenderer;

    private void Start()
    {
        _canStep = false;
        //_cellTaken = false;
    }

    private void OnMouseEnter()
    {
        ChangeColor(_hoverColor);
    }

    private void Update()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        if (globalvars.selected == _prefabCharacter && globalvars.selected != null)
        {
            ChangeColor(Color.blue);
        }
    }

    private void OnMouseDown()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        if (Menu.Paused) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (_canStep)
        {
            //var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
            switch (globalvars.cellAction)
            {
                case "setEnemy":
                    setEntity(_cellPosition.x, _cellPosition.y, globalvars.enemies[globalvars.setType]);
                    refresh();
                    break;
                case "setWall":
                    setEntity(_cellPosition.x, _cellPosition.y, globalvars.wall);
                    refresh();
                    break;
                case "Replace":
                    Replace(_cellPosition.x, _cellPosition.y, globalvars.selected);
                    refresh();
                    break;
                case "Move":
                    Move(_cellPosition.x, _cellPosition.y, globalvars.selected);
                    ClearSelect();
                    globalvars.cellAction = "Done";
                    GameObject.Find("ButtonMove").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
                case "Attack":
                    StartCoroutine(Attack(gameObject));
                    ClearSelect();
                    globalvars.cellAction = "Done";
                    GameObject.Find("ButtonAttack").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
                case "Cast":
                    StartCoroutine(Cast(gameObject));
                    ClearSelect();
                    globalvars.cellAction = "Done";
                    GameObject.Find("ButtonCast").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    break;
            }
        }
        if (_prefabCharacter)
        {
            if (_prefabCharacter.GetComponent<Character>())
            {
                if (_prefabCharacter.GetComponent<Character>()._canRemove)
                {
                    Destroy(_prefabCharacter);
                    _cellTaken = false;
                    _prefabCharacter = null;
                    ChangeColor(_standartColor);
                } else  if (_prefabCharacter.GetComponent<Character>()._canReplace)
                {
                    var cells = GameObject.FindGameObjectsWithTag("Cell");
                    foreach (var cell in cells)
                    {
                        cell.GetComponent<Cell>().ChangeColor(_standartColor);
                    }

                    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (var enemy in enemies)
                    {
                        enemy.GetComponent<Character>()._canReplace = false;
                    }

                    var heroes = GameObject.FindGameObjectsWithTag("Hero");
                    foreach (var hero in heroes)
                    {
                        hero.GetComponent<Character>()._canReplace = false;
                    }
                    //var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
                    globalvars.cellAction = "Replace";
                    globalvars.cellColor = new Color(1f, 0.99f, 0.15f, 0);
                    globalvars.setType = 0;
                    globalvars.selected = _prefabCharacter;
                    stepMap();
                } else if (!_canStep && SceneManager.GetActiveScene().buildIndex == 2)
                {
                    var hero = _prefabCharacter.GetComponent<Character>();
                    GameObject.Find("TextAttr").GetComponent<Text>().text =
                        $"Имя {_prefabCharacter.name} \r\n" +
                        $"Класс: {hero.Class} \r\n" +
                        $"Здоровье: {hero.Health} \r\n" +
                        $"Скорость: {hero.Speed} \r\n" +
                        $"Броня: {hero.Armor} \r\n";
                }
            }
        }
    }

    public void Replace(int x, int y, GameObject entity)
    {
        GameObject cell = GameObject.Find($"Cell X_{x} Y_{y}");
        Character character = entity.GetComponent<Character>();
        var oldcell = GameObject.Find($"Cell X_{character.cellPostion.x} Y_{character.cellPostion.y}");
        oldcell.GetComponent<Cell>()._cellTaken = false;
        oldcell.GetComponent<Cell>()._prefabCharacter = null;
        character.cellPostion = new Vector2Int(x, y);
        entity.transform.position = new Vector3(cell.GetComponent<MeshRenderer>().bounds.center.x, cell.GetComponent<MeshRenderer>().bounds.center.y, cell.GetComponent<MeshRenderer>().bounds.center.z);
        _cellTaken = true;
        _prefabCharacter = entity;
    }

    public void Move(int x, int y, GameObject entity)
    {
        GameObject cell = GameObject.Find($"Cell X_{x} Y_{y}");
        Character character = entity.GetComponent<Character>();
        if (character.MoveCount > 0)
        {
            var oldcell = GameObject.Find($"Cell X_{character.cellPostion.x} Y_{character.cellPostion.y}");
            oldcell.GetComponent<Cell>()._cellTaken = false;
            oldcell.GetComponent<Cell>()._prefabCharacter = null;
            character.cellPostion = new Vector2Int(x, y);
            entity.transform.position = new Vector3(cell.GetComponent<MeshRenderer>().bounds.center.x, cell.GetComponent<MeshRenderer>().bounds.center.y, cell.GetComponent<MeshRenderer>().bounds.center.z);
            character.MoveCount--;
            _cellTaken = true;
            _prefabCharacter = entity;
            GameObject.Find("TextMoveCount").GetComponent<Text>().text = character.MoveCount.ToString();
        }
    }

    public IEnumerator Attack(GameObject cell)
    {
        GameObject.Find("TextCube").GetComponent<Text>().text = "";
        GameObject.Find("TextResult").GetComponent<Text>().text = "";
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        var hero = cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Character>();
        globalvars.selected.GetComponent<Character>().MainMove = false;
        GameObject.Find("TextMainMove").GetComponent<Text>().text = globalvars.selected.GetComponent<Character>().MainMove ? "" + 1 : "" + 0;
        globalvars.selected.GetComponent<Animator>().Play("Attack");
        yield return new WaitForSeconds((globalvars.selected.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length/3)-1);
        int cube = Random.Range(1, 20);
        GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
        yield return new WaitForSeconds(1);
        if (cube == 1)
        {
            GameObject.Find("TextResult").GetComponent<Text>().text = $"Промах";
        } else if (cube == 20)
        {
            cube = Random.Range(2, 8);
            hero.Health -= cube;
            GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
            GameObject.Find("TextResult").GetComponent<Text>().text = $"Критический урон";
            cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Animator>().Play("GetAttack");
        }
        else
        {
            if (cube > hero.Armor)
            {
                cube = Random.Range(1, 4);
                hero.Health -= cube;
                GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
                GameObject.Find("TextResult").GetComponent<Text>().text = $"Урон";
                cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Animator>().Play("GetAttack");
            } else
            {
                GameObject.Find("TextResult").GetComponent<Text>().text = $"Промах";
            }
        }
        if (hero.Health <= 0)
        {
            var heroobj = cell.GetComponent<Cell>()._prefabCharacter;
            heroobj.GetComponent<Animator>().Play("Die");
            yield return new WaitForSeconds((globalvars.selected.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length));
            Destroy(heroobj);
            cell.GetComponent<Cell>()._cellTaken = false;
            cell.GetComponent<Cell>()._prefabCharacter = null;
            GameObject.Find("Canvas").GetComponent<UnitControl>().queue.Remove(heroobj);
            Destroy(heroobj);
        }
    }

    public IEnumerator Cast(GameObject cell)
    {
        GameObject.Find("TextCube").GetComponent<Text>().text = "";
        GameObject.Find("TextResult").GetComponent<Text>().text = "";
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        globalvars.selected.GetComponent<Animator>().Play("Attack");
        var hero = cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Character>();
        globalvars.selected.GetComponent<Character>().MainMove = false;
        GameObject.Find("TextMainMove").GetComponent<Text>().text = globalvars.selected.GetComponent<Character>().MainMove ? "" + 1 : "" + 0;
        yield return new WaitForSeconds((globalvars.selected.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length / 3) - 1);
        var spellobj = globalvars.spells.Where(s => s.GetComponent<Skill>().Class == globalvars.selected.GetComponent<Character>().Class).First();
        var position = new Vector3(cell.GetComponent<MeshRenderer>().bounds.center.x, 1, cell.GetComponent<MeshRenderer>().bounds.center.z);
        var setobj = Instantiate(spellobj.gameObject, position, spellobj.transform.rotation, GameObject.Find("GridCells").GetComponent<GridGenerator>()._parent);
        setobj.GetComponent<CFX_AutoDestructShuriken>().OnlyDeactivate = false;
        setobj.gameObject.SetActive(true);
        var spell = spellobj.GetComponent<Skill>();
        if (spell.Effect == "Урон")
        {
            int cube = Random.Range(1, 20);
            GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
            yield return new WaitForSeconds(1);
            if (cube == 1)
            {
                GameObject.Find("TextResult").GetComponent<Text>().text = $"Промах";
            }
            else if (cube == 20)
            {
                cube = Random.Range(2, 8);
                hero.Health -= cube;
                GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
                GameObject.Find("TextResult").GetComponent<Text>().text = $"Критический урон";
                cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Animator>().Play("GetAttack");
            }
            else
            {
                if (cube > hero.Armor)
                {
                    cube = Random.Range(1, 4);
                    hero.Health -= cube;
                    GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
                    GameObject.Find("TextResult").GetComponent<Text>().text = $"Урон";
                    cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Animator>().Play("GetAttack");
                }
                else
                {
                    GameObject.Find("TextResult").GetComponent<Text>().text = $"Промах";
                }
            }
        } else if (spell.Effect == "Исцеление")
        {
            int cube = Random.Range(1, 4);
            if ((hero.Health + cube) > hero.MaxHealth)
            {
                hero.Health = hero.MaxHealth;
            }
            else
            {
                hero.Health += cube;
            }
            GameObject.Find("TextCube").GetComponent<Text>().text = $"{cube}";
            GameObject.Find("TextResult").GetComponent<Text>().text = $"Исцеление";
        }
        if (hero.Health <= 0)
        {
            var heroobj = cell.GetComponent<Cell>()._prefabCharacter;
            heroobj.GetComponent<Animator>().Play("Die");
            yield return new WaitForSeconds((globalvars.selected.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length));
            GameObject.Find("Canvas").GetComponent<UnitControl>().queue.Remove(heroobj);
            Destroy(heroobj);
            cell.GetComponent<Cell>()._cellTaken = false;
            cell.GetComponent<Cell>()._prefabCharacter = null;
        }
    }


    public void setEntity(int x, int y, GameObject entity)
    {
        GameObject cell = GameObject.Find($"Cell X_{x} Y_{y}");
        var position = new Vector3(cell.GetComponent<MeshRenderer>().bounds.center.x, cell.GetComponent<MeshRenderer>().bounds.center.y, cell.GetComponent<MeshRenderer>().bounds.center.z);
        var setobj = Instantiate(entity, position, Quaternion.identity, GameObject.Find("GridCells").GetComponent<GridGenerator>()._parent);
        setobj.SetActive(true);
        var wall = setobj.GetComponent<Wall>() ?? null;
        if (wall == null)
        {
            var character = setobj.GetComponent<Character>() ?? null;
            character.cellPostion = new Vector2Int(x, y);
            setobj.name = character.Name + "_" + GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
        else
        {
            wall.cellPostion = new Vector2Int(x, y);
        }
        _prefabCharacter = setobj;
        _cellTaken = true;
    }

    public void refresh()
    {
        ClearSelect();
        stepMap();
    }

    public void stepMap()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        var cells = GameObject.FindGameObjectsWithTag("Cell");
        foreach (var cell in cells)
        {
            if (!cell.GetComponent<Cell>()._cellTaken)
            {
                cell.GetComponent<Cell>()._canStep = true;
                cell.GetComponent<MeshRenderer>().material.color = globalvars.cellColor;
            }
        }
    }

    private void ClearSelect()
    {
        var cells = GameObject.FindGameObjectsWithTag("Cell");
        foreach (var cell in cells)
        {
            var cellvar = cell.GetComponent<Cell>();
            cellvar._canStep = false;
            cell.GetComponent<MeshRenderer>().material.color = cell.GetComponent<Cell>()._standartColor;
        }
    }

    private void OnMouseExit()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();

        if (_canStep)
        {
            ChangeColor(globalvars.cellColor);
        }
        else
        {
            ChangeColor(_standartColor);
        }
    }

    public void ChangeColor(Color color)
    {
        if (Menu.Paused) return;
        _meshRenderer.material.color = color;
    }
}
