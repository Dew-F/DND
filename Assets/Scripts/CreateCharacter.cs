using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{
    [SerializeField] InputField Name;

    [SerializeField] Button ButtonPrevCharacter;
    [SerializeField] Button ButtonNextCharacter;
    [SerializeField] List<GameObject> Models;
    int CurrentCharacter = 0;

    [SerializeField] Button ButtonPrevClass;
    [SerializeField] Button ButtonNextClass;
    [SerializeField] Text Class;
    int CurrentClass = 0;

    List<Skill> skills = new List<Skill>();

    [SerializeField] Button ButtonMinusSpeed;
    [SerializeField] Button ButtonPlusSpeed;
    [SerializeField] Text TextSpeed;
    int Speed = 30;
    string SpeedText = "Скорость - ";
    int MaxSpeed = int.MaxValue;

    [SerializeField] Button ButtonPrevSave;
    [SerializeField] Button ButtonNextSave;
    public List<GameObject> save = new List<GameObject>();
    int CurrentSave = 0;
    [SerializeField] Text SaveText;

    public void Start()
    {
        Restart();
    }

    public void Save()
    {
        if (CurrentSave != 0)
        {
            Destroy(save[CurrentSave - 1].gameObject);
            save.RemoveAt(CurrentSave - 1);
        }

        GameObject duplicate = Instantiate(Models[CurrentCharacter]);

        duplicate.AddComponent<Character>();
        duplicate.GetComponent<Character>().Name = Name.text;
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        duplicate.GetComponent<Character>().Class = menuvars.classes.Where(x => x.Name == Class.text).First().Name;
        duplicate.GetComponent<Character>().Speed = Speed;
        duplicate.GetComponent<Character>().Armor = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Strength = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Agility = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Intelligence = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Wisdom = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Charisma = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Constitution = Random.Range(1, 10);
        duplicate.GetComponent<Character>().Health = 10;
        duplicate.GetComponent<Character>().MaxHealth = 10;
        duplicate.GetComponent<Character>().Model = Models[CurrentCharacter].name;

        if (CurrentSave == 0)
        {
            duplicate.name = $"Hero_{save.Count + 1}";
        } else
        {
            duplicate.name = $"Hero_{CurrentSave}";
        }

        DontDestroyOnLoad(duplicate);
        duplicate.SetActive(false);
        duplicate.tag = "Hero";

        save.Add(duplicate);

        RestartIndex(save.Count);
    }

    public void CheckIndex(Button Prev, Button Next, int i, int count)
    {
        if (i > 0)
        {
            Prev.enabled = true;
        }
        else
        {
            Prev.enabled = false;
        }
        if (i < count - 1)
        {
            Next.enabled = true;
        }
        else
        {
            Next.enabled = false;
        }
    }

    public void PrevModel()
    {
        Models[CurrentCharacter].SetActive(false);
        CurrentCharacter--;
        Models[CurrentCharacter].SetActive(true);
        CheckIndex(ButtonPrevCharacter, ButtonNextCharacter, CurrentCharacter, Models.Count);
    }

    public void NextModel()
    {
        Models[CurrentCharacter].SetActive(false);
        CurrentCharacter++;
        Models[CurrentCharacter].SetActive(true);
        CheckIndex(ButtonPrevCharacter, ButtonNextCharacter, CurrentCharacter, Models.Count);
    }

    public void PrevClass()
    {
        CurrentClass--;
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
    }

    public void NextClass()
    {
        CurrentClass++;
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
    }

    public void MinusSpeed()
    {
        Speed--;
        TextSpeed.text = SpeedText + Speed;
        CheckIndex(ButtonMinusSpeed, ButtonPlusSpeed, Speed - 1, MaxSpeed);
    }

    public void PlusSpeed()
    {
        Speed++;
        TextSpeed.text = SpeedText + Speed;
        CheckIndex(ButtonMinusSpeed, ButtonPlusSpeed, Speed - 1, MaxSpeed);
    }

    public void PrevSave()
    {
        CurrentSave--;
        if (CurrentSave != 0)
        {
            SaveText.text = save[CurrentSave - 1].GetComponent<Character>().Name;
        }
        else
        {
            SaveText.text = "Новый персонаж";
        }
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        EditCharacter();
    }

    public void NextSave()
    {
        CurrentSave++;
        SaveText.text = save[CurrentSave - 1].GetComponent<Character>().Name;
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        EditCharacter();
    }

    public void EditCharacter()
    {
        if (CurrentSave != 0)
        {
            RestartIndex(CurrentSave);
        }
        else
        {
            Restart();
        }
    }

    public void Restart()
    {
        CurrentSave = 0;
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        Models[CurrentCharacter].SetActive(false);
        CurrentCharacter = 0;
        Models[CurrentCharacter].SetActive(true);
        CheckIndex(ButtonPrevCharacter, ButtonNextCharacter, CurrentCharacter, Models.Count);
        Name.text = "";
        CurrentClass = 0;
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
        Speed = 30;
        TextSpeed.text = SpeedText + Speed;
        CheckIndex(ButtonMinusSpeed, ButtonPlusSpeed, Speed - 1, MaxSpeed);
        SaveText.text = "Новый персонаж";
    }

    public void RestartIndex(int i)
    {
        CurrentSave = i;
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        Models[CurrentCharacter].SetActive(false);
        CurrentCharacter = Models.FindIndex(x => x.name == save[CurrentSave - 1].GetComponent<Character>().Model);
        Models[CurrentCharacter].SetActive(true);
        CheckIndex(ButtonPrevCharacter, ButtonNextCharacter, CurrentCharacter, Models.Count);
        Name.text = save[CurrentSave - 1].GetComponent<Character>().Name;
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        CurrentClass = menuvars.classes.FindIndex(x => x.Name == save[CurrentSave - 1].GetComponent<Character>().Class);
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
        Speed = save[CurrentSave - 1].GetComponent<Character>().Speed;
        TextSpeed.text = SpeedText + Speed;
        CheckIndex(ButtonMinusSpeed, ButtonPlusSpeed, Speed - 1, MaxSpeed);
        SaveText.text = save[CurrentSave - 1].GetComponent<Character>().Name;
    }

    public void DeleteSave()
    {
        save.RemoveAt(CurrentSave - 1);
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
    }
}
