using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> heroes = new List<GameObject>();
    public List<ParticleSystem> spells = new List<ParticleSystem>();
    public List<Profession> classes = new List<Profession>();
    public GameObject wall;
    public int size = 10;
    public Vector2Int cellClicked;
    public string cellAction;
    public Color cellColor;
    public int setType;
    public GameObject selected;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
