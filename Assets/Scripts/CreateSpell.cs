using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class CreateSpell : MonoBehaviour
{
    [SerializeField] InputField Name;
    [SerializeField] Text NameError;

    [SerializeField] Button ButtonPrevModel;
    [SerializeField] Button ButtonNextModel;
    [SerializeField] List<ParticleSystem> Models;
    int CurrentModel = 0;

    [SerializeField] Button ButtonPrevEffect;
    [SerializeField] Button ButtonNextEffect;
    [SerializeField] Text Effect;
    List<string> effects = new List<string>() {"Исцеление", "Урон"};
    int CurrentEffect = 0;

    [SerializeField] Button ButtonPrevChar;
    [SerializeField] Button ButtonNextChar;
    [SerializeField] Text Char;
    List<string> chars = new List<string>() { "Сила", "Интеллект", "Ловкость"};
    int CurrentChar = 0;

    [SerializeField] Button ButtonPrevTarget;
    [SerializeField] Button ButtonNextTarget;
    [SerializeField] Text Target;
    List<string> targets = new List<string>() { "На себя", "На других"};
    int CurrentTarget = 0;

    [SerializeField] Button ButtonMinusCount;
    [SerializeField] Button ButtonPlusCount;
    [SerializeField] Text Count;
    int CountValue = 1;
    string CountText = "Количество - ";

    [SerializeField] Button ButtonMinusRadius;
    [SerializeField] Button ButtonPlusRadius;
    [SerializeField] Text Radius;
    int RadiusValue = 1;
    string RadiusText = "Радиус - ";

    [SerializeField] Button ButtonMinusRange;
    [SerializeField] Button ButtonPlusRange;
    [SerializeField] Text Range;
    int RangeValue = 1;
    string RangeText = "Дальность - ";

    [SerializeField] Button ButtonMinusCastTime;
    [SerializeField] Button ButtonPlusCastTime;
    [SerializeField] Text CastTime;
    int CastTimeValue = 1;
    string CastTimeText = "Время каста - ";

    [SerializeField] Button ButtonMinusDuration;
    [SerializeField] Button ButtonPlusDuration;
    [SerializeField] Text Duration;
    int DurationValue = 1;
    string DurationText = "Длительность - ";

    [SerializeField] Button ButtonPrevSave;
    [SerializeField] Button ButtonNextSave;
    public List<ParticleSystem> save = new List<ParticleSystem>();
    int CurrentSave = 0;
    [SerializeField] Text SaveText;

    [SerializeField] Button ButtonPrevClass;
    [SerializeField] Button ButtonNextClass;
    [SerializeField] Text Class;
    int CurrentClass = 0;

    public void Start()
    {
        Restart();
    }

    public void Save()
    {
        if (Name.text != "")
        {
            if (CurrentSave != 0)
            {
                Destroy(save[CurrentSave - 1].gameObject);
                save.RemoveAt(CurrentSave - 1);
            }

            ParticleSystem duplicate = Instantiate(Models[CurrentModel]);

            duplicate.AddComponent<Skill>();
            duplicate.GetComponent<Skill>().Name = Name.text;
            duplicate.GetComponent<Skill>().Effect = Effect.text;
            duplicate.GetComponent<Skill>().MainCharacteristic = Char.text;
            duplicate.GetComponent<Skill>().Target = Target.text;
            duplicate.GetComponent<Skill>().Count = CountValue;
            duplicate.GetComponent<Skill>().Radius = RadiusValue;
            duplicate.GetComponent<Skill>().Range = RangeValue;
            duplicate.GetComponent<Skill>().CastingTime = CastTimeValue;
            duplicate.GetComponent<Skill>().Duration = DurationValue;
            duplicate.GetComponent<Skill>().Model = Models[CurrentModel].name;
            Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
            duplicate.GetComponent<Skill>().Class = menuvars.classes.Where(x => x.Name == Class.text).First().Name;

            if (CurrentSave == 0)
            {
                duplicate.name = $"Spell_{save.Count + 1}";
            }
            else
            {
                duplicate.name = $"Spell_{CurrentSave}";
            }

            DontDestroyOnLoad(duplicate);
            duplicate.gameObject.SetActive(false);

            save.Add(duplicate);

            RestartIndex(save.Count);

            NameError.text = "";
        } else
        {
            NameError.text = "*требуется ввести название";
        }
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
        Models[CurrentModel].gameObject.SetActive(false);
        CurrentModel--;
        Models[CurrentModel].gameObject.SetActive(true);
        CheckIndex(ButtonPrevModel, ButtonNextModel, CurrentModel, Models.Count);
    }

    public void NextModel()
    {
        Models[CurrentModel].gameObject.SetActive(false);
        CurrentModel++;
        Models[CurrentModel].gameObject.SetActive(true);
        CheckIndex(ButtonPrevModel, ButtonNextModel, CurrentModel, Models.Count);
    }

    public void PrevEffect()
    {
        CurrentEffect--;
        Effect.text = effects[CurrentEffect];
        CheckIndex(ButtonPrevEffect, ButtonNextEffect, CurrentEffect, effects.Count);
    }

    public void NextEffect()
    {
        CurrentEffect++;
        Effect.text = effects[CurrentEffect];
        CheckIndex(ButtonPrevEffect, ButtonNextEffect, CurrentEffect, effects.Count);
    }

    public void PrevChar()
    {
        CurrentChar--;
        Char.text = chars[CurrentChar];
        CheckIndex(ButtonPrevChar, ButtonNextChar, CurrentChar, chars.Count);
    }

    public void NextChar()
    {
        CurrentChar++;
        Char.text = chars[CurrentChar];
        CheckIndex(ButtonPrevChar, ButtonNextChar, CurrentChar, chars.Count);
    }

    public void PrevTarget()
    {
        CurrentTarget--;
        Target.text = targets[CurrentTarget];
        CheckIndex(ButtonPrevTarget, ButtonNextTarget, CurrentTarget, targets.Count);
    }

    public void NextTarget()
    {
        CurrentTarget++;
        Target.text = targets[CurrentTarget];
        CheckIndex(ButtonPrevTarget, ButtonNextTarget, CurrentTarget, targets.Count);
    }

    public void MinusCount()
    {
        CountValue--;
        Count.text = CountText + CountValue;
        CheckIndex(ButtonMinusCount, ButtonPlusCount, CountValue - 1, int.MaxValue);
    }

    public void PlusCount()
    {
        CountValue++;
        Count.text = CountText + CountValue;
        CheckIndex(ButtonMinusCount, ButtonPlusCount, CountValue - 1, int.MaxValue);
    }

    public void PlusRadius()
    {
        RadiusValue++;
        Radius.text = RadiusText + RadiusValue;
        CheckIndex(ButtonMinusRadius, ButtonPlusRadius, RadiusValue - 1, int.MaxValue);
    }


    public void MinusRadius()
    {
        RadiusValue--;
        Radius.text = RadiusText + RadiusValue;
        CheckIndex(ButtonMinusRadius, ButtonPlusRadius, RadiusValue - 1, int.MaxValue);
    }

    public void PlusRange()
    {
        RangeValue++;
        Range.text = RangeText + RangeValue;
        CheckIndex(ButtonMinusRange, ButtonPlusRange, RangeValue - 1, int.MaxValue);
    }


    public void MinusRange()
    {
        RangeValue--;
        Range.text = RangeText + RangeValue;
        CheckIndex(ButtonMinusRange, ButtonPlusRange, RangeValue - 1, int.MaxValue);
    }

    public void PlusCastTime()
    {
        CastTimeValue++;
        CastTime.text = CastTimeText + CastTimeValue;
        CheckIndex(ButtonMinusCastTime, ButtonPlusCastTime, CastTimeValue - 1, int.MaxValue);
    }


    public void MinusCastTime()
    {
        CastTimeValue--;
        CastTime.text = CastTimeText + CastTimeValue;
        CheckIndex(ButtonMinusCastTime, ButtonPlusCastTime, CastTimeValue - 1, int.MaxValue);
    }

    public void PlusDuration()
    {
        DurationValue++;
        Duration.text = DurationText + DurationValue;
        CheckIndex(ButtonMinusDuration, ButtonPlusDuration, DurationValue - 1, int.MaxValue);
    }


    public void MinusDuration()
    {
        DurationValue--;
        Duration.text = DurationText + DurationValue;
        CheckIndex(ButtonMinusDuration, ButtonPlusDuration, DurationValue - 1, int.MaxValue);
    }

    public void PrevSave()
    {
        CurrentSave--;
        if (CurrentSave != 0)
        {
            SaveText.text = save[CurrentSave - 1].GetComponent<Skill>().Name;
        }
        else
        {
            SaveText.text = "Новый скилл";
        }
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        Edit();
    }

    public void NextSave()
    {
        CurrentSave++;
        SaveText.text = save[CurrentSave - 1].GetComponent<Skill>().Name;
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        Edit();
    }

    public void PrevClass()
    {
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        CurrentClass--;
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
    }

    public void NextClass()
    {
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        CurrentClass++;
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
    }

    public void Edit()
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
        Models[CurrentModel].gameObject.SetActive(false);
        CurrentModel = 0;
        Models[CurrentModel].gameObject.SetActive(true);
        CheckIndex(ButtonPrevModel, ButtonNextModel, CurrentModel, Models.Count);
        Name.text = "";
        Effect.text = effects[0];
        Char.text = chars[0];
        Target.text = targets[0];
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        Class.text = menuvars.classes[0].Name;
        CheckIndex(ButtonPrevEffect, ButtonNextEffect, CurrentEffect, effects.Count);
        CheckIndex(ButtonPrevChar, ButtonNextChar, CurrentChar, chars.Count);
        CheckIndex(ButtonPrevTarget, ButtonNextTarget, CurrentTarget, targets.Count);
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
        CountValue = 1;
        Count.text = CountText + CountValue;
        CheckIndex(ButtonMinusCount, ButtonPlusCount, CountValue - 1, int.MaxValue);
        RadiusValue = 1;
        Radius.text = RadiusText + RadiusValue;
        CheckIndex(ButtonMinusRadius, ButtonPlusRadius, RadiusValue - 1, int.MaxValue);
        RangeValue = 1;
        Range.text = RangeText + RangeValue;
        CheckIndex(ButtonMinusRange, ButtonPlusRange, RangeValue - 1, int.MaxValue);
        CastTimeValue = 1;
        CastTime.text = CastTimeText + CastTimeValue;
        CheckIndex(ButtonMinusCastTime, ButtonPlusCastTime, CastTimeValue - 1, int.MaxValue);
        DurationValue = 1;
        Duration.text = DurationText + DurationValue;
        CheckIndex(ButtonMinusDuration, ButtonPlusDuration, DurationValue - 1, int.MaxValue);
        SaveText.text = "Новый скилл";
    }

    public void RestartIndex(int i)
    {
        CurrentSave = i;
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
        Models[CurrentModel].gameObject.SetActive(false);
        CurrentModel = Models.FindIndex(x => x.name == save[CurrentSave - 1].GetComponent<Skill>().Model);
        Models[CurrentModel].gameObject.SetActive(true);
        CheckIndex(ButtonPrevModel, ButtonNextModel, CurrentModel, Models.Count);
        Name.text = save[CurrentSave - 1].GetComponent<Skill>().Name;
        CurrentEffect = effects.FindIndex(x => x == save[CurrentSave - 1].GetComponent<Skill>().Effect);
        Effect.text = effects[CurrentEffect];
        CheckIndex(ButtonPrevEffect, ButtonNextEffect, CurrentEffect, effects.Count);
        CurrentChar = chars.FindIndex(x => x == save[CurrentSave - 1].GetComponent<Skill>().MainCharacteristic);
        Char.text = chars[CurrentChar];
        CheckIndex(ButtonPrevChar, ButtonNextChar, CurrentChar, chars.Count);
        CurrentTarget = targets.FindIndex(x => x == save[CurrentSave - 1].GetComponent<Skill>().Target);
        Target.text = targets[CurrentTarget];
        CheckIndex(ButtonPrevTarget, ButtonNextTarget, CurrentTarget, targets.Count);
        Menu menuvars = GameObject.Find("Canvas").GetComponent("Menu") as Menu;
        CurrentClass = menuvars.classes.FindIndex(x => x.Name == save[CurrentSave - 1].GetComponent<Skill>().Class);
        Class.text = menuvars.classes[CurrentClass].Name;
        CheckIndex(ButtonPrevClass, ButtonNextClass, CurrentClass, menuvars.classes.Count);
        CountValue = save[CurrentSave - 1].GetComponent<Skill>().Count;
        Count.text = CountText + CountValue;
        CheckIndex(ButtonMinusCount, ButtonPlusCount, CountValue - 1, int.MaxValue);
        RadiusValue = save[CurrentSave - 1].GetComponent<Skill>().Radius;
        Radius.text = RadiusText + RadiusValue;
        CheckIndex(ButtonMinusRadius, ButtonPlusRadius, RadiusValue - 1, int.MaxValue);
        RangeValue = save[CurrentSave - 1].GetComponent<Skill>().Range;
        Range.text = RangeText + RangeValue;
        CheckIndex(ButtonMinusRange, ButtonPlusRange, RangeValue - 1, int.MaxValue);
        CastTimeValue = save[CurrentSave - 1].GetComponent<Skill>().CastingTime;
        CastTime.text = CastTimeText + CastTimeValue;
        CheckIndex(ButtonMinusCastTime, ButtonPlusCastTime, CastTimeValue - 1, int.MaxValue);
        DurationValue = save[CurrentSave - 1].GetComponent<Skill>().Duration;
        Duration.text = DurationText + DurationValue;
        CheckIndex(ButtonMinusDuration, ButtonPlusDuration, DurationValue - 1, int.MaxValue);
        SaveText.text = save[CurrentSave - 1].GetComponent<Skill>().Name;
    }

    public void DeleteSave()
    {
        save.RemoveAt(CurrentSave - 1);
        CheckIndex(ButtonPrevSave, ButtonNextSave, CurrentSave, save.Count + 1);
    }
}