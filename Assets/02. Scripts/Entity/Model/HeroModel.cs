using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HeroModel : IDamageable
{
    private ReactiveProperty<int> _currentHp;
    public IReadOnlyReactiveProperty<int> CurrentHp => _currentHp;
    
    private ReactiveProperty<bool> _isDead;
    public IReadOnlyReactiveProperty<bool> IsDead => _isDead;
    
    public HeroConfig Config { get; }

    public HeroModel(HeroConfig config)
    {
        Config = config;
        _currentHp = new ReactiveProperty<int>(config.MaxHealth);
        _isDead = new ReactiveProperty<bool>(false);
    }
    
    public void TakeDamage(int damage)
    {
        if (_isDead.Value) return;
        
        _currentHp.Value = Mathf.Max(0, _currentHp.Value - damage);
        
        if (_currentHp.Value <= 0)
        {
            _isDead.Value = true;
        }
    }
}
