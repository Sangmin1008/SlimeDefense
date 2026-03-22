using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager
{
    private readonly EnemyRegistry _enemyRegistry;
    
    private readonly Dictionary<ProjectileView, ObjectPool<ProjectileView>> _pools = new Dictionary<ProjectileView, ObjectPool<ProjectileView>>();
    
    public ProjectileManager(EnemyRegistry enemyRegistry)
    {
        _enemyRegistry = enemyRegistry;
    }

    public void SpawnProjectile(ProjectileView prefab, Vector3 startPos, int damage, float speed, float maxDist,
        EnemyModel targetModel, EnemyView targetView)
    {
        if (prefab == null) return;

        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new ObjectPool<ProjectileView>(
                createFunc: () => Object.Instantiate(prefab),
                actionOnGet: view => view.gameObject.SetActive(true),
                actionOnRelease: view => view.gameObject.SetActive(false),
                actionOnDestroy: view => { if (view) Object.Destroy(view.gameObject); }
            );
        }

        ProjectileView view = _pools[prefab].Get();
        view.transform.position = startPos;

        ProjectileModel model = new ProjectileModel(damage, speed, maxDist, targetModel, targetView);
        ProjectilePresenter presenter = new ProjectilePresenter(model, view, _enemyRegistry);
        
        presenter.OnComplete += p => 
        {
            _pools[prefab].Release(p.View);
        };

        presenter.Initialize();
    }
}
