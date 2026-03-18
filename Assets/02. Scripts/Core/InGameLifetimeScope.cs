using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{
    [Header("Configurations")]
    [SerializeField] private EnemyConfig normalEnemyConfig;
    [SerializeField] private HeroConfig heroConfig;

    [Header("Prefabs")]
    [SerializeField] private EnemyView enemyViewPrefab;
    [SerializeField] private HeroView heroViewPrefab;
    
    [Header("Map Data")]
    [SerializeField] private WaypointPath waypointPath;
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(normalEnemyConfig);
        builder.RegisterInstance(heroConfig);
        builder.RegisterInstance(enemyViewPrefab);
        builder.RegisterInstance(heroViewPrefab);
        
        builder.Register<EnemyRegistry>(Lifetime.Scoped);
        builder.RegisterEntryPoint<EnemySpawner>().AsSelf();
        builder.RegisterEntryPoint<GameTest>();
        
        builder.RegisterComponent(waypointPath);
    }
}
