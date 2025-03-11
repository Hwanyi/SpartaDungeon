using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float maxValue;
    private float curValue;
    public float CurValue { get => curValue; }

    public float passiveValue;

    public Image UIBar;

    private void Start()
    {
        curValue = maxValue;
    }

    private void Update()
    {
        UIBar.fillAmount = GetRate();
    }

    public void AddValue(float value)
    {
        curValue += value;
        if(curValue > maxValue)
        {
            curValue = maxValue;
        }
    }

    public void SubValue(float value)
    {
        curValue -= value;
        if (curValue <= 0)
        {
            curValue = 0;
        }
    }

    public float GetRate()
    {
        return curValue / maxValue;
    }
}
