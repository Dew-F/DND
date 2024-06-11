using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class UnitControl : MonoBehaviour
{
    int sid;
    public List<GameObject> queue = new List<GameObject>();

    public void Start()
    {
        queue.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        queue.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        Selected(queue[0]);
        sid = 0;
    }

    private void Selected(GameObject unit)
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        Character character = unit.GetComponent<Character>();
        GameObject.Find("TextName").GetComponent<Text>().text = unit.name;
        character.MoveCount = character.Speed / 5;
        character.MainMove = true;
        GameObject.Find("TextMoveCount").GetComponent<Text>().text = character.MoveCount.ToString();
        GameObject.Find("TextMainMove").GetComponent<Text>().text = character.MainMove ? ""+1 : ""+0;
        globalvars.selected = unit;
        ClearSelect();
        globalvars.cellAction = "Done";
        GameObject.Find("ButtonMove").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        GameObject.Find("ButtonAttack").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        GameObject.Find("ButtonCast").GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void Move()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        if (globalvars.cellAction == "Done")
        {
            var unit = queue[sid];
            Character character = unit.GetComponent<Character>();
            if (character.MoveCount > 0)
            {
                globalvars.cellAction = "Move";
                globalvars.cellColor = new Color(1f, 0.99f, 0.15f, 0);
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        var cell = GameObject.Find($"Cell X_{character.cellPostion.x + x} Y_{character.cellPostion.y + y}");
                        if (cell)
                        {
                            if (!cell.GetComponent<Cell>()._cellTaken)
                            {
                                cell.GetComponent<Cell>()._canStep = true;
                                cell.GetComponent<MeshRenderer>().material.color = globalvars.cellColor;
                            }
                        }
                    }
                }
                GameObject.Find("ButtonMove").GetComponent<Image>().color = new Color(0, 1, 1, 1);
            }
        } else if (globalvars.cellAction == "Move")
        {
            ClearSelect();
            globalvars.cellAction = "Done";
            GameObject.Find("ButtonMove").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    public void Attack()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        var unit = queue[sid];
        Character character = unit.GetComponent<Character>();
        if (globalvars.cellAction == "Done")
        {
            if (character.MainMove)
            {
                globalvars.cellAction = "Attack";
                globalvars.cellColor = new Color(0.5f, 0.1f, 0f, 0);
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        var cell = GameObject.Find($"Cell X_{character.cellPostion.x + x} Y_{character.cellPostion.y + y}");
                        if (cell)
                        {
                            if (cell.GetComponent<Cell>()._cellTaken)
                            {
                                if (!cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Wall>())
                                {
                                    if (cell.GetComponent<Cell>()._prefabCharacter != unit)
                                    {
                                        cell.GetComponent<Cell>()._canStep = true;
                                        cell.GetComponent<MeshRenderer>().material.color = globalvars.cellColor;
                                        Refresh();
                                    }
                                }
                            }
                        }
                    }
                }
                GameObject.Find("ButtonAttack").GetComponent<Image>().color = new Color(0, 1, 1, 1);
            }
        } else if (globalvars.cellAction == "Attack")
        {
            ClearSelect();
            globalvars.cellAction = "Done";
            GameObject.Find("ButtonAttack").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    public void Cast()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        var unit = queue[sid];
        Character character = unit.GetComponent<Character>();
        if (globalvars.cellAction == "Done")
        {
            if (character.MainMove)
            {
                globalvars.cellAction = "Cast";
                globalvars.cellColor = new Color(0.28f, 0f, 0.32f, 0);
                var spellobj = globalvars.spells.Where(s => s.GetComponent<Skill>().Class == character.Class).First();
                var spell = spellobj.GetComponent<Skill>();
                for (int x = -spell.Range; x <= spell.Range; x++)
                {
                    for (int y = -spell.Range; y <= spell.Range; y++)
                    {
                        var cell = GameObject.Find($"Cell X_{character.cellPostion.x + x} Y_{character.cellPostion.y + y}");
                        if (cell)
                        {
                            if (cell.GetComponent<Cell>()._cellTaken)
                            {
                                if (!cell.GetComponent<Cell>()._prefabCharacter.GetComponent<Wall>())
                                {
                                    cell.GetComponent<Cell>()._canStep = true;
                                    cell.GetComponent<MeshRenderer>().material.color = globalvars.cellColor;
                                    Refresh();
                                }
                            }
                        }
                    }
                }
                GameObject.Find("ButtonCast").GetComponent<Image>().color = new Color(0, 1, 1, 1);
            }
        }
        else if (globalvars.cellAction == "Cast")
        {
            ClearSelect();
            globalvars.cellAction = "Done";
            GameObject.Find("ButtonCast").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    private void Refresh()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        var cells = GameObject.FindGameObjectsWithTag("Cell");
        foreach (var cell in cells)
        {
            if (cell.GetComponent<Cell>()._canStep == true)
            {
                cell.GetComponent<Cell>().ChangeColor(globalvars.cellColor);
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

    public void EndMove()
    {
        if (sid+1 < queue.Count)
        {
            sid++;
        } else
        {
            sid = 0;
        }
        Selected(queue[sid]);
        ClearSelect();
    }
}
