using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    [SerializeField] private Transform hpBar;
    [SerializeField] private Transform projectileSpawnPoint;

    private float _initialScaleX = 1f;

    private void Awake()
    {
        if (hpBar)
        {
            _initialScaleX = hpBar.localScale.x;
        }
    }

    public void UpdateHpBar(float health)
    {
        if (!hpBar) return;
        
        Vector3 scale = hpBar.localScale;
        scale.x = health * _initialScaleX;
        hpBar.localScale = scale;
    }

    public void LookAtTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.z = 0;
        
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
