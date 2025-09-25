using UnityEngine;

[CreateAssetMenu(menuName = "Stat Menu/Default Stat Setup", fileName = "Default Stat")]
public class StatSetupSO : ScriptableObject
{

    [Header("Resources")]
    public float MaxHealth = 100;

    [Header("Offense")]
    public float Damage = 10;
    public float CritChance;
    public float CritPower;

    [Header("Defense")]
    public float Armor;
    public float Evasion;

    [Header("Major")]
    public float Strength;
    public float Agility;
    public float Vitality;

}
