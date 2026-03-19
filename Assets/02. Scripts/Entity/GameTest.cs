using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class GameTest : IInitializable
{
    private readonly HeroView _prefab;
    private readonly EnemyRegistry _registry;
    private readonly HeroModel _heroModel;

    public GameTest(HeroView prefab, EnemyRegistry registry, HeroModel heroModel)
    {
        _prefab = prefab;
        _registry = registry;
        _heroModel = heroModel;
    }

    public void Initialize()
    {
        HeroView view = Object.Instantiate(_prefab, new Vector3(2.5f, 0, 1.5f), Quaternion.identity);
        
        
        HeroPresenter presenter = new HeroPresenter(_heroModel, view, _registry);
        presenter.Initialize();
        
        Debug.Log("영웅 소환 완료");
    }
}
