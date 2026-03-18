using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class GameTest : IInitializable
{
    private readonly HeroConfig _config;
    private readonly HeroView _prefab;
    private readonly EnemyRegistry _registry;

    public GameTest(HeroConfig config, HeroView prefab, EnemyRegistry registry)
    {
        _config = config;
        _prefab = prefab;
        _registry = registry;
    }

    public void Initialize()
    {
        HeroView view = Object.Instantiate(_prefab, new Vector3(2.5f, 0, 1.5f), Quaternion.identity);
        
        HeroModel model = new HeroModel(_config);
        
        HeroPresenter presenter = new HeroPresenter(model, view, _registry);
        presenter.Initialize();
        
        Debug.Log("영웅 소환 완료");
    }
}
