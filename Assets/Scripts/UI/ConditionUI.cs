using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionUI : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.Player.condition.uiCondition = this;
    }
}
