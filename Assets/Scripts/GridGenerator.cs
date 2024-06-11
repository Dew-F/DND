using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int _gridSize;
    [SerializeField] private Cell _prefabCell;
    float _offset;
    [SerializeField] public Transform _parent;

    public void GenerateGrid()
    {
        var cellsize = _prefabCell.GetComponent<MeshRenderer>().bounds.size;

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                var position = new Vector3(x * (cellsize.x + _offset), 0, y * (cellsize.z + _offset));

                var cell = Instantiate(_prefabCell, position, Quaternion.identity, _parent);

                cell._cellPosition = new Vector2Int(x, y);

                cell.name = $"Cell X_{x} Y_{y}";
            }
        }

        GlobalVars global = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();

        foreach (var hero in global.heroes)
        {
            GenerateCharacters(hero);
        }
    }

    int charcount = 0;

    public void GenerateCharacters(GameObject hero)
    {
        int x = charcount;
        int y = 0;
        GameObject cell = GameObject.Find($"Cell X_{x} Y_{y}");
        var position = new Vector3(cell.GetComponent<MeshRenderer>().bounds.center.x, cell.GetComponent<MeshRenderer>().bounds.center.y, cell.GetComponent<MeshRenderer>().bounds.center.z);
        var character = Instantiate(hero, position, Quaternion.identity, _parent);
        character.GetComponent<Character>().cellPostion = new Vector2Int(x, y);
        character.name = GameObject.Find("GlobalVars").GetComponent<GlobalVars>().heroes[charcount].GetComponent<Character>().Name;
        character.SetActive(true);
        cell.GetComponent<Cell>()._prefabCharacter = character;
        cell.GetComponent<Cell>()._cellTaken = true;
        charcount++;
    }

    void Start()
    {
        _offset = 0.0F;
        _gridSize.Set(GameObject.Find("GlobalVars").GetComponent<GlobalVars>().size, GameObject.Find("GlobalVars").GetComponent<GlobalVars>().size);
        GenerateGrid();
    }
}
