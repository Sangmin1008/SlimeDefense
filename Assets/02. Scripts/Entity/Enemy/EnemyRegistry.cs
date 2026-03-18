using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry
{
    private readonly Dictionary<EnemyModel, EnemyView> _activeEnemies = new Dictionary<EnemyModel, EnemyView>();

    public void Register(EnemyModel model, EnemyView view)
    {
        if (_activeEnemies.ContainsKey(model)) return;
        _activeEnemies.Add(model, view);
    }

    public void Unregister(EnemyModel model)
    {
        _activeEnemies.Remove(model);
    }

    public bool TryGetClosestEnemy(Vector3 centerPos, float radius, out EnemyModel closestModel, out Vector3 closestPos)
    {
        closestModel = null;
        closestPos = Vector3.zero;
        
        float minDistanceSqr = radius * radius; 
        bool found = false;

        foreach (var enemy in _activeEnemies)
        {
            EnemyModel model = enemy.Key;
            EnemyView view = enemy.Value;

            if (!view) continue;
            if (model.IsDead.Value) continue;

            Vector3 enemyPos = view.transform.position;
            float distanceSqr = (enemyPos - centerPos).sqrMagnitude;

            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                closestModel = model;
                closestPos = enemyPos;
                found = true;
            }
        }

        return found;
    }
}
