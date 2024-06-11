using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject PanelMain;
    [SerializeField] private GameObject PanelCharacter;
    [SerializeField] private GameObject PanelSpell;
    [SerializeField] private GameObject PanelEnenemy;
    [SerializeField] private GameObject PanelMap;
    public List<Profession> classes = new List<Profession>();

    public static bool Paused = false;

    public void StartGame()
    {
        GlobalVars global = GameObject.Find("GlobalVars").GetComponent<GlobalVars>();
        global.heroes = PanelCharacter.GetComponent<CreateCharacter>().save;
        global.spells = PanelSpell.GetComponent<CreateSpell>().save;
        global.enemies = PanelEnenemy.GetComponent<CreateEnemy>().save;
        global.classes = classes;
        SceneManager.LoadScene(1);
    }

    public void Start()
    {
        Profession profession = new Profession();
        profession.MainCharacteristic = "Сила";
        profession.Name = "Воин";
        classes.Add(profession);
        profession = new Profession();
        profession.MainCharacteristic = "Ловкость";
        profession.Name = "Лучник";
        classes.Add(profession);
        profession = new Profession();
        profession.MainCharacteristic = "Интеллект";
        profession.Name = "Маг";
        classes.Add(profession);
    }


    public void OpenMain(GameObject panel)
    {
        panel.SetActive(false);
        PanelMain.SetActive(true);
    }

    public void OpenSpell(GameObject panel)
    {
        panel.SetActive(false);
        PanelSpell.SetActive(true);
    }

    public void OpenCharacter(GameObject panel)
    {
        panel.SetActive(false);
        PanelCharacter.SetActive(true);
    }

    public void OpenEnemy(GameObject panel)
    {
        panel.SetActive(false);
        PanelEnenemy.SetActive(true);
    }

    public void OpenMap(GameObject panel)
    {
        panel.SetActive(false);
        PanelMap.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
