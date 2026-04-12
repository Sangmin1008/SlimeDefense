using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using VContainer.Unity;
using UnityEngine;

public class CommanderPresenter : IInitializable, IDisposable
{
    private readonly CommanderModel _model;
    private readonly CommanderView _view;
    private readonly EnemyRegistry _enemyRegistry;
    private readonly ProjectileSpawner _projectileSpawner;
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private IDisposable _attackTimerDisposable;
    
    public CommanderPresenter(CommanderModel model, CommanderView view, EnemyRegistry enemyRegistry, ProjectileSpawner projectileSpawner)
    {
        _model = model;
        _view = view;
        _enemyRegistry = enemyRegistry;
        _projectileSpawner = projectileSpawner;
    }
    
    public void Initialize()
    {
        _model.IsDead
            .Where(isDead => isDead)
            .Subscribe(_ => HandleDeath())
            .AddTo(_disposables);
        
        StartAttackLoop();
    }
    
    private void StartAttackLoop()
    {
        _attackTimerDisposable = Observable.Interval(TimeSpan.FromSeconds(_model.Config.AttackCooldown))
            .Where(_ => !_model.IsDead.Value)
            .Subscribe(_ => TryAttack())
            .AddTo(_disposables);
    }
    
    private void TryAttack()
    {
        if (_enemyRegistry.TryGetClosestEnemy(_view.transform.position, _model.Config.AttackRange, out EnemyModel targetModel, out Vector3 targetPosition))
        {
            if (_enemyRegistry.TryGetView(targetModel, out EnemyView targetView))
            {
                _projectileSpawner.SpawnProjectile(
                    _model.Config.ProjectilePrefab.GetComponent<ProjectileView>(), 
                    _view.transform.position, 
                    _model.Config.AttackPower,
                    _model.Config.ProjectileSpeed,
                    _model.Config.ProjectileMaxDistance,
                    targetModel,
                    targetView
                );
            }
        }
    }
    
    private void HandleDeath()
    {
        _attackTimerDisposable?.Dispose();
        UnityEngine.Object.Destroy(_view.gameObject);
        Debug.Log("지휘관 죽음");
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}
