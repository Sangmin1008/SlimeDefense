using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class GameTest : IInitializable
{
    private readonly CommanderView _prefab;
    private readonly EnemyRegistry _registry;
    private readonly CommanderModel _commanderModel;

    public GameTest(CommanderView prefab, EnemyRegistry registry, CommanderModel commanderModel)
    {
        _prefab = prefab;
        _registry = registry;
        _commanderModel = commanderModel;
    }

    public void Initialize()
    {
        CommanderView view = Object.Instantiate(_prefab, new Vector3(2.5f, 0, 1.5f), Quaternion.identity);
        
        
        CommanderPresenter presenter = new CommanderPresenter(_commanderModel, view, _registry);
        presenter.Initialize();
        
        Debug.Log("지휘관 소환 완료");
    }
}
