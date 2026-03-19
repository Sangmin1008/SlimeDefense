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
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private IDisposable _attackTimerDisposable;
    
    public CommanderPresenter(CommanderModel model, CommanderView view, EnemyRegistry enemyRegistry)
    {
        _model = model;
        _view = view;
        _enemyRegistry = enemyRegistry;
    }
    
    public void Initialize()
    {
        _model.CurrentHp
            .Subscribe(hp => 
            {
                float normalizedHp = (float)hp / _model.Config.MaxHealth;
                _view.UpdateHpBar(normalizedHp);
            })
            .AddTo(_disposables);

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
            _view.LookAtTarget(targetPosition);
            targetModel.TakeDamage(_model.Config.AttackPower);
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
