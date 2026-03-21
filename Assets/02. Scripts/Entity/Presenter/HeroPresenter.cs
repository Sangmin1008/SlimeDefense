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
    
    private CompositeDisposable _disposables = new CompositeDisposable();
    
    public HeroModel Model => _model;
    public HeroView View => _view;
    
    public HeroPresenter(HeroModel model, HeroView view, EnemyRegistry enemyRegistry)
    {
        _model = model;
        _view = view;
        _enemyRegistry = enemyRegistry;
        
        _view.SetGizmoRange(_model.Config.AttackRange);
    }
    
    public void Initialize()
    {
        Observable.Interval(TimeSpan.FromSeconds(_model.Config.AttackCooldown))
            .Subscribe(_ => TryAttack())
            .AddTo(_disposables);
    }
    
    private void TryAttack()
    {
        if (_enemyRegistry.TryGetClosestEnemy(_view.transform.position, _model.Config.AttackRange, out EnemyModel targetModel, out Vector3 targetPosition))
        {
            targetModel.TakeDamage(_model.CurrentAttackPower);
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
