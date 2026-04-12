using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class HeroPresenter : IInitializable, IDisposable
{
    private readonly HeroModel _model;
    private readonly HeroView _view;
    private readonly EnemyRegistry _enemyRegistry;
    private readonly ProjectileSpawner _projectileSpawner;
    
    private CompositeDisposable _disposables = new CompositeDisposable();
    
    public HeroModel Model => _model;
    public HeroView View => _view;
    
    public HeroPresenter(HeroModel model, HeroView view, EnemyRegistry enemyRegistry, ProjectileSpawner projectileSpawner)
    {
        _model = model;
        _view = view;
        _enemyRegistry = enemyRegistry;
        _projectileSpawner = projectileSpawner;
        
        _view.SetGizmoRange(_model.Config.AttackRange);
    }
    
    public void Initialize()
    {
        Observable.Interval(TimeSpan.FromSeconds(_model.Config.AttackCooldown))
            .TakeUntilDestroy(_view)
            .Subscribe(_ => TryAttack())
            .AddTo(_disposables);
    }
    
    private void TryAttack()
    {
        if (_enemyRegistry.TryGetClosestEnemy(_view.transform.position, _model.Config.AttackRange, out EnemyModel targetModel, out Vector3 targetPosition))
        {
            // targetModel.TakeDamage(_model.CurrentAttackPower);
            if (_enemyRegistry.TryGetView(targetModel, out EnemyView targetView))
            {
                _projectileSpawner.SpawnProjectile(
                    _model.Config.ProjectilePrefab.GetComponent<ProjectileView>(), 
                    _view.transform.position, 
                    _model.CurrentAttackPower,
                    _model.Config.ProjectileSpeed,
                    _model.Config.ProjectileMaxDistance,
                    targetModel,
                    targetView
                );
            }
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
