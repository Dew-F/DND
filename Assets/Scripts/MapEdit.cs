using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapEdit : MonoBehaviour
{
    bool createEnemyActive = false;
    public void createEnemy()
    {
        ClearSelect();
        createWallActive = false;
        RemoveActive = false;
        if (!createEnemyActive)
        {
            var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
            globalvars.cellAction = "setEnemy";
            globalvars.cellColor = new Color(1f, 0.99f, 0.15f, 0);
            globalvars.setType = 0;
            stepMap();
            createEnemyActive = true;
        } else
        {
            ClearSelect();
            createEnemyActive = false;
        }
    }

    bool createWallActive = false;
    public void createWall()
    {
        ClearSelect();
        createEnemyActive = false;
        RemoveActive = false;
        if (!createWallActive)
        {
            var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
            globalvars.cellAction = "setWall";
            globalvars.cellColor = new Color(1f, 0.99f, 0.15f, 0);
            globalvars.setType = 0;
            stepMap();
            createWallActive = true;
        }
        else
        {
            ClearSelect();
            createWallActive = false;
        }
    }

    bool RemoveActive = false;
    public void Remove()
    {
        ClearSelect();
        createEnemyActive = false;
        createWallActive = false;
        if (!RemoveActive)
        {
            var walls = GameObject.FindGameObjectsWithTag("Wall");
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var wall in walls)
            {
                wall.GetComponent<Wall>()._canRemove = true;
            }
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Character>()._canRemove = true;
            }
            RemoveActive = true;
        }
        else
        {
            ClearSelect();
            RemoveActive = false;
            var walls = GameObject.FindGameObjectsWithTag("Wall");
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var wall in walls)
            {
                wall.GetComponent<Wall>()._canRemove = false;
            }
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Character>()._canRemove = false;
            }
        }
    }

    bool ReplaceActive = false;
    public void Replace()
    {
        ClearSelect();
        createEnemyActive = false;
        createWallActive = false;
        RemoveActive = false;
        if (!ReplaceActive)
        {
            var heroes = GameObject.FindGameObjectsWithTag("Hero");
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var hero in heroes)
            {
                hero.GetComponent<Character>()._canReplace = true;
            }
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Character>()._canReplace = true;
            }
            ReplaceActive = true;
        }
        else
        {
            var heroes = GameObject.FindGameObjectsWithTag("Hero");
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var hero in heroes)
            {
                hero.GetComponent<Character>()._canReplace = false;
            }
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Character>()._canReplace = false;
            }
            ClearSelect();
            ReplaceActive = false;
        }
    }

    public void stepMap()
    {
        var globalvars = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        var cells = GameObject.FindGameObjectsWithTag("Cell");
        foreach(var cell in cells)
        {
            if (!cell.GetComponent<Cell>()._cellTaken)
            {
                cell.GetComponent<Cell>()._canStep = true;
                cell.GetComponent<MeshRenderer>().material.color = globalvars.cellColor;
            }
        }
    }

    public void End()
    {
        ClearSelect();
        var map = GameObject.Find("GridCells");
        DontDestroyOnLoad(map);
        SceneManager.LoadScene(2);
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
}
