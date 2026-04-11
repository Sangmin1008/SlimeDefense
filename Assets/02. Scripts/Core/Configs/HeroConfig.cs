using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroGrade
{
    Normal = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3
}

public enum HeroType
{
    TypaA,
    TypeB,
}

[CreateAssetMenu(fileName = "HeroConfig", menuName = "ScriptableObject/HeroConfig")]
public class HeroConfig : ScriptableObject
{
    [Header("Settings")]
    public string HeroName;
    public HeroGrade Grade;
    public HeroType Type;
    
    [Header("Prefabs")]
    public GameObject Prefab;

    [Header("Attack Settings")]
    public int AttackPower;
    public float AttackRange;
    public float AttackCooldown;
    
    [Header("Projectile Settings")]
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float ProjectileMaxDistance;
}
