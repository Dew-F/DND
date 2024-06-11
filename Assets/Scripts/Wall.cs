using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool _canRemove;
    public Vector2Int cellPostion;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _delColor;
    [SerializeField] private Color _standartColor;

    private void Start()
    {
        _canRemove = false;
    }

    private void Update()
    {
        if (_canRemove)
        {
            ChangeColor(_delColor);
        } else
        {
            ChangeColor(_standartColor);
        }
    }

    private void OnMouseDown()
    {
        if (_canRemove)
        {
            Destroy(this.gameObject);
            GameObject cell = GameObject.Find($"Cell X_{cellPostion.x} Y_{cellPostion.y}");
            cell.GetComponent<Cell>()._cellTaken = false;
        }
    }

    private void ChangeColor(Color color)
    {
        if (Menu.Paused) return;
        _meshRenderer.material.color = color;
    }
}
