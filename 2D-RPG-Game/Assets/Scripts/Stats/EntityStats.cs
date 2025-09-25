using System.Collections;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public StatSetupSO StatSetup;
    public Stat Health;
    public Stat CurrentHealth;
    public StatMajor Major;
    public StatOffensive Offensive;
    public StatDefencive Defencive;

    void Awake()
    {
        ApplyDefaultStatSetup();
    }
    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = Offensive.Damage.GetValue();
        float bonusDamage = Major.Strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = Offensive.CritChance.GetValue();
        float bonusCritChance = Major.Agility.GetValue() * 0.3f;
        float totalCritChance = baseCritChance + bonusCritChance;

        float critPower = Offensive.CritPower.GetValue();
        float bonusCritPower = Major.Strength.GetValue() * .5f;
        float totalCritPower = (critPower + bonusCritPower) / 100;

        isCrit = RNG.CalculateRNG(totalCritChance);
        float damageResult = isCrit ? totalBaseDamage * (1 + totalCritPower) : totalBaseDamage;
        Debug.Log("Source: " + transform.name + " Damage: " + damageResult);
        return damageResult;

    }

    public float GetArmorMitigation()
    {
        float baseArmor = Defencive.Armor.GetValue();
        float bonusArmor = Major.Vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float mitigation = totalArmor / (totalArmor + 100f);
        return mitigation > .80f ? .80f : mitigation;
    }
    public float GetMaxHealth()
    {
        float hp = Health.GetValue();
        float bonus = Major.Vitality.GetValue() * 5f;
        return hp + bonus;
    }
    public float GetEvasion()
    {
        float baseEvasion = Defencive.Evasion.GetValue();
        float bonus = Major.Agility.GetValue() / 2f;
        return (baseEvasion + bonus) > 90 ? 90 : (baseEvasion + bonus);
    }
    public void ApplyDefaultStatSetup()
    {
        if (StatSetup == null)
            return;
        Health.SetBaseValue(StatSetup.MaxHealth);
        Major.Strength.SetBaseValue(StatSetup.Strength);
        Major.Vitality.SetBaseValue(StatSetup.Vitality);
        Major.Agility.SetBaseValue(StatSetup.Agility);

        Offensive.CritChance.SetBaseValue(StatSetup.CritChance);
        Offensive.CritPower.SetBaseValue(StatSetup.CritPower);
        Offensive.Damage.SetBaseValue(StatSetup.Damage);

        Defencive.Armor.SetBaseValue(StatSetup.Armor);
        Defencive.Evasion.SetBaseValue(StatSetup.Evasion);
    }
    public Stat GetStatType(StatType a)
    {
        switch (a)
        {
            case StatType.Maxhealth: return Health;
            case StatType.CurrentHealth: return CurrentHealth;
            case StatType.Strength: return Major.Strength;
            case StatType.Agility: return Major.Agility;
            case StatType.Vitality: return Major.Vitality;
            case StatType.Damage: return Offensive.Damage;
            case StatType.CritChance: return Offensive.CritChance;
            case StatType.CritPower: return Offensive.CritPower;
            case StatType.Armor: return Defencive.Armor;
            case StatType.Evasion: return Defencive.Evasion;
            default:
                Debug.LogWarning("No Type of Buff");
                return null;
        }

    }
}

public enum StatType
{
    Maxhealth,
    CurrentHealth,
    Strength,
    Agility,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    Armor,
    Evasion,

}
