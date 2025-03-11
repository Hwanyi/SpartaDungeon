using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public ConditionUI uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float recoveryHpTime;
    private float curDamageTime;

    public float recoveryStaminaTime;
    private float curStaminaTime;

    private void Awake()
    {
        curDamageTime = Time.time;
        curStaminaTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - curDamageTime > recoveryHpTime)
            health.AddValue(health.passiveValue);
        
        if (Time.time - curStaminaTime > recoveryStaminaTime)
            stamina.AddValue(stamina.passiveValue);
    }

    public void GetDamage(float value)
    {
        health.SubValue(value);
        curDamageTime = Time.time;
        if(health.CurValue <= 0f)
        {
            //die
        }
    }

    public bool UseStamina(float value)
    {
        if(stamina.CurValue - value <= 0f)
        {
            return false;
        }

        curStaminaTime = Time.time;
        stamina.SubValue(value);

        return true;
    }
}
