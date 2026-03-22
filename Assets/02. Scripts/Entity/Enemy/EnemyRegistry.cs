using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry
{
    private readonly Dictionary<EnemyModel, EnemyView> _modelToView = new Dictionary<EnemyModel, EnemyView>();
    private readonly Dictionary<EnemyView, EnemyModel> _viewToModel = new Dictionary<EnemyView, EnemyModel>();

    public void Register(EnemyModel model, EnemyView view)
    {
        if (!_modelToView.ContainsKey(model)) _modelToView.Add(model, view);
        if (!_viewToModel.ContainsKey(view)) _viewToModel.Add(view, model);
    }

    public void Unregister(EnemyModel model)
    {
        if (_modelToView.TryGetValue(model, out EnemyView view))
        {
            _viewToModel.Remove(view);
            _modelToView.Remove(model);
        }
    }
    
    public bool TryGetModel(EnemyView view, out EnemyModel model)
    {
        return _viewToModel.TryGetValue(view, out model);
    }

    public bool TryGetView(EnemyModel model, out EnemyView view)
    {
        return _modelToView.TryGetValue(model, out view);
    }

    public bool TryGetClosestEnemy(Vector3 centerPos, float radius, out EnemyModel closestModel, out Vector3 closestPos)
    {
        closestModel = null;
        closestPos = Vector3.zero;
        
        float minDistanceSqr = radius * radius; 
        bool found = false;

        foreach (var enemy in _modelToView)
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
