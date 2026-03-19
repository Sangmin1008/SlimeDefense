using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

public class EnemySpawner : IInitializable, IDisposable
{
    private readonly StageConfig _stageConfig;
    private readonly WaveModel _waveModel;
    private readonly HeroModel _heroModel;
    
    private readonly EnemyView _enemyView;
    private readonly EnemyRegistry _registry;
    
    private ObjectPool<EnemyView> _enemyPool;
    private CompositeDisposable _disposables = new CompositeDisposable();
    
    private CancellationTokenSource _cts;
    

    public EnemySpawner(StageConfig stageConfig, WaveModel waveModel, HeroModel heroModel, EnemyView enemyView, EnemyRegistry registry)
    {
        _stageConfig = stageConfig;
        _waveModel = waveModel;
        _heroModel = heroModel;
        
        _enemyView = enemyView;
        _registry = registry;
        
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
    
    
    
    public void Initialize()
    {
        _cts = new CancellationTokenSource();
        WaveRoutineAsync(_cts.Token).Forget();
    }
    
    
    private async UniTaskVoid WaveRoutineAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < _stageConfig.Waves.Count; i++)
        {
            if (_waveModel.IsGameOver.Value) return;

            _waveModel.CurrentWaveIndex.Value = i;
            WaveConfig currentWave = _stageConfig.Waves[i];

            await UniTask.Delay(TimeSpan.FromSeconds(currentWave.DelayTimeBeforeWave), cancellationToken: cancellationToken);

            for (int j = 0; j < currentWave.SpawnCount; j++)
            {
                if (_waveModel.IsGameOver.Value) return;
                
                SpawnEnemy(currentWave.EnemyType, currentWave.PathData);
                
                await UniTask.Delay(TimeSpan.FromSeconds(currentWave.SpawnInterval), cancellationToken: cancellationToken);
            }

            await UniTask.WaitUntil(() => _waveModel.AliveEnemiesCount.Value == 0 || _waveModel.IsGameOver.Value, cancellationToken: cancellationToken);
        }
    }
    
    
    private void SpawnEnemy(EnemyConfig enemyConfig, PathDataSO pathData)
    {
        EnemyView view = _enemyPool.Get();
        EnemyModel model = new EnemyModel(enemyConfig);
        EnemyPresenter presenter = new EnemyPresenter(model, view);
        presenter.Initialize();
        
        _registry.Register(model, view);

        _waveModel.AliveEnemiesCount.Value++;

        model.IsDead
            .Where(isDead => isDead)
            .Subscribe(_ => 
            {
                _registry.Unregister(model);
                presenter.Dispose();
                _waveModel.AliveEnemiesCount.Value--;
                _enemyPool.Release(view);
            })
            .AddTo(view);
        
        model.OnEscaped
            .Subscribe(_ => 
            {
                _registry.Unregister(model);
                presenter.Dispose();
                _waveModel.AliveEnemiesCount.Value--;

                _heroModel.TakeDamage(enemyConfig.AttackPower);
                Debug.Log("적이 도달, 체력 감소");

                _enemyPool.Release(view);
            })
            .AddTo(view);

        presenter.StartMovement(pathData.PathPositions);
    }


    public void Dispose()
    {
        _disposables.Dispose();
        _enemyPool?.Dispose();
    }
}
