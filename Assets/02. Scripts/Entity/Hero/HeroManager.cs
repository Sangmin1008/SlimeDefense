using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroManager
{
    private readonly HashSet<HeroPresenter> _activeHeroes = new HashSet<HeroPresenter>();

    public void AddHero(HeroPresenter hero)
    {
        _activeHeroes.Add(hero);
    }

    public void RemoveHero(HeroPresenter hero)
    {
        _activeHeroes.Remove(hero);
        hero.Dispose();
    }

    public void TryUpgradeHero(HeroPresenter hero)
    {
        HeroGrade currentGrade = hero.Model.Config.Grade;
        
        if (currentGrade == HeroGrade.Legendary) return;
        
        HeroPresenter otherHero = _activeHeroes.FirstOrDefault(h => h.Model.Config.Grade == currentGrade && h != hero);
        if (otherHero == null) return;

        Vector3 position = hero.View.transform.position;
        
        RemoveHero(hero);
        RemoveHero(otherHero);
        
        HeroGrade nextGrade = currentGrade + 1;
        // SpawnHero(nextGrade, position);
    }
}
