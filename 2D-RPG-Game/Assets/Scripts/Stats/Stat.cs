using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] public float _baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private float finalValue;

    private bool needToCalculate;
    public float GetValue()
    {
        if (needToCalculate)
        {
            needToCalculate = false;
            return CalculateRealValueOfBuff();
        }

        return _baseValue;
    }
    public void Buff(string buffName, float BuffValue)
    {
        modifiers.Add(new StatModifier(buffName, BuffValue));
        needToCalculate = true;
    }
    public void DeBuff(string BuffName)
    {
        modifiers.RemoveAll(modifier => modifier.BuffName == BuffName);
        needToCalculate = true;
    }

    public float CalculateRealValueOfBuff()
    {
        float basevalue = _baseValue;
        foreach (var a in modifiers)
        {
            if (!a.isCalculated)
            {
                basevalue += a.BuffValue;
                a.isCalculated = true;
            }
        }
        _baseValue = basevalue;
        return _baseValue;
    }
    public float SetBaseValue(float baseValue) => _baseValue = baseValue;

}
[Serializable]
public class StatModifier
{
    public string BuffName;
    public float BuffValue;
    public bool isCalculated;
    public StatModifier(string buffName, float BuffValue)
    {
        this.BuffName = buffName;
        this.BuffValue = BuffValue;
        this.isCalculated = false;
    }

}


