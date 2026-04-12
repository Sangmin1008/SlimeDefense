using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner
{
    private readonly EnemyRegistry _enemyRegistry;
    private readonly Dictionary<ProjectileView, ObjectPool<ProjectilePresenter>> _pools = new Dictionary<ProjectileView, ObjectPool<ProjectilePresenter>>();
    
    public ProjectileSpawner(EnemyRegistry enemyRegistry)
    {
        _enemyRegistry = enemyRegistry;
    }

    public void SpawnProjectile(ProjectileView prefab, Vector3 startPos, int damage, float speed, float maxDist, EnemyModel targetModel, EnemyView targetView)
    {
        if (prefab == null) return;

        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new ObjectPool<ProjectilePresenter>(
                createFunc: () => CreatePresenter(prefab),
                actionOnGet: p => p.View.gameObject.SetActive(true),
                actionOnRelease: p => 
                {
                    p.View.gameObject.SetActive(false);
                    p.Release();
                },
                actionOnDestroy: p => 
                {
                    p.Dispose();
                    if (p.View) Object.Destroy(p.View.gameObject);
                }
            );
        }

        ProjectilePresenter presenter = _pools[prefab].Get();
        presenter.ResetDataAndFire(startPos, damage, speed, maxDist, targetModel, targetView.gameObject);
    }

    private ProjectilePresenter CreatePresenter(ProjectileView prefab)
    {
        ProjectileView view = Object.Instantiate(prefab);
        ProjectileModel model = new ProjectileModel(0, 0, 0, null, null); 
        
        ProjectilePresenter presenter = new ProjectilePresenter(model, view, _enemyRegistry);
        
        presenter.OnComplete += p => _pools[prefab].Release(p);

        return presenter;
    }
}
