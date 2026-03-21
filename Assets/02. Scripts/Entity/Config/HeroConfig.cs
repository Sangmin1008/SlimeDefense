using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroGrade
{
    Normal,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "HeroConfig", menuName = "ScriptableObject/HeroConfig")]
public class HeroConfig : ScriptableObject
{
    public string HeroName;
    public HeroGrade Grade;

    [Header("Attack Settings")]
    public int AttackPower;
    public float AttackRange;
    public float AttackCooldown;
}
