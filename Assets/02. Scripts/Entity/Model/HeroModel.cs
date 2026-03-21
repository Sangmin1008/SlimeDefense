using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModel
{
    public HeroConfig Config { get; }
    public int CurrentAttackPower { get; }

    public HeroModel(HeroConfig config)
    {
        Config = config;
        CurrentAttackPower = config.AttackPower;
    }
}
