using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{
    [SerializeField] Button ButtonMinusSize;
    [SerializeField] Button ButtonPlusSize;
    [SerializeField] Button ButtonMinusSizeMul;
    [SerializeField] Button ButtonPlusSizeMul;
    [SerializeField] Text TextSize;
    int Size = 10;
    string SizeText = "Размер - ";
    int MaxSize = int.MaxValue;

    public void Start()
    {
        Restart();
    }

    public void Save()
    {
        GameObject.Find("GlobalVars").GetComponent<GlobalVars>().size = Size;
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

    public void MinusSize()
    {
        Size--;
        TextSize.text = SizeText + Size;
        CheckIndex(ButtonMinusSize, ButtonPlusSize, Size - 1, MaxSize);
    }

    public void PlusSize()
    {
        Size++;
        TextSize.text = SizeText + Size;
        CheckIndex(ButtonMinusSize, ButtonPlusSize, Size - 1, MaxSize);
    }

    public void PlusSizeMul()
    {
        Size += 10;
        TextSize.text = SizeText + Size;
        CheckIndex(ButtonMinusSizeMul, ButtonPlusSizeMul, Size - 10, MaxSize);
    }

    public void MinusSizeMul()
    {
        Size -= 10;
        TextSize.text = SizeText + Size;
        CheckIndex(ButtonMinusSizeMul, ButtonPlusSizeMul, Size - 10, MaxSize);
    }

    public void Restart()
    {
        Size = 10;
        TextSize.text = SizeText + Size;
        CheckIndex(ButtonMinusSize, ButtonPlusSize, Size - 1, MaxSize);
    }
}
