using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

public class EnemySpawner : ITickable, IDisposable
{
    private readonly IObjectResolver _container;
    private readonly EnemyView _enemyView;
    private readonly EnemyConfig _enemyConfig; // TEST용
    private readonly EnemyRegistry _registry;
    private readonly WaypointPath _waypointPath;
    private ObjectPool<EnemyView> _enemyPool;
    

    private float _spawnTimer;
    private readonly float _spawnInterval = 2f;

    public EnemySpawner(IObjectResolver container, EnemyView enemyView, EnemyConfig enemyConfig, EnemyRegistry registry, WaypointPath waypointPath)
    {
        _container = container;
        _enemyView = enemyView;
        _enemyConfig = enemyConfig;
        _registry = registry;
        _waypointPath = waypointPath;
        
        InitializePool();
    }

    private void InitializePool()
    {
        _enemyPool = new ObjectPool<EnemyView>(
            createFunc: () => UnityEngine.Object.Instantiate(_enemyView),
            actionOnGet: view => view.gameObject.SetActive(true),
            actionOnRelease: view => view.gameObject.SetActive(false),
            actionOnDestroy: view => 
            {
                if (view)
                {
                    UnityEngine.Object.Destroy(view.gameObject);
                }
            },
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 100
        );
    }
    
    public void Tick()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }
    }
    
    
    private void SpawnEnemy()
    {
        EnemyView view = _enemyPool.Get();
        EnemyModel model = new EnemyModel(_enemyConfig);
        EnemyPresenter presenter = new EnemyPresenter(model, view);
        presenter.Initialize();
        
        _registry.Register(model, view);

        model.IsDead
            .Where(isDead => isDead)
            .Subscribe(_ => 
            {
                _registry.Unregister(model);
                presenter.Dispose();
                
                _enemyPool.Release(view);
            })
            .AddTo(view);
        
        model.OnEscaped
            .Subscribe(_ => 
            {
                _registry.Unregister(model);
                presenter.Dispose();
                
                Debug.Log("적이 탈출함");

                _enemyPool.Release(view);
            })
            .AddTo(view);

        List<Vector3> path = _waypointPath.GetPathPositions();
        presenter.StartMovement(path);
    }


    public void Dispose()
    {
        _enemyPool?.Dispose();
    }
}
