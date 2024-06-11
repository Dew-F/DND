using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Vector2Int cellPostion;

    [SerializeField] public string Name;
    [SerializeField] public string Model;
    [SerializeField] public string Class;

    [SerializeField] public int Strength;
    [SerializeField] public int Agility;
    [SerializeField] public int Constitution;
    [SerializeField] public int Intelligence;
    [SerializeField] public int Wisdom;
    [SerializeField] public int Charisma;
    [SerializeField] public int Health;
    [SerializeField] public int MaxHealth;

    [SerializeField] public int Speed;
    [SerializeField] public int Armor;

    public int MoveCount;
    public bool MainMove;

    private Color _delColor = Color.red;
    private Color _standartColor = new Color(1, 1, 1, 0);
    private Color _moveColor = Color.green;
    private MeshRenderer _meshRenderer;
    public bool _canRemove;
    public bool _canReplace;

    private void Start()
    {
        _canRemove = false;
        _canReplace = false;
    }

    private void Update()
    {
        Cell cell = GameObject.Find($"Cell X_{cellPostion.x} Y_{cellPostion.y}").GetComponent<Cell>();
        if (_canRemove)
        {
            cell.ChangeColor(_delColor);
        }
        else if (_canReplace)
        {
            cell.ChangeColor(_moveColor);
        }
    }
}
