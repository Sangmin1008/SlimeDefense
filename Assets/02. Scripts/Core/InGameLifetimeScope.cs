using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{
    [Header("Configurations")]
    [SerializeField] private EnemyConfig normalEnemyConfig;
    [SerializeField] private HeroConfig heroConfig;
    [SerializeField] private StageConfig stageConfig;

    [Header("Prefabs")]
    [SerializeField] private EnemyView enemyViewPrefab;
    [SerializeField] private HeroView heroViewPrefab;
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(normalEnemyConfig);
        builder.RegisterInstance(heroConfig);
        builder.RegisterInstance(stageConfig);
        
        builder.RegisterInstance(enemyViewPrefab);
        builder.RegisterInstance(heroViewPrefab);
        
        builder.Register<WaveModel>(Lifetime.Scoped);
        builder.Register<HeroModel>(Lifetime.Scoped).WithParameter(heroConfig);
        builder.Register<EnemyRegistry>(Lifetime.Scoped);
        builder.RegisterEntryPoint<EnemySpawner>().AsSelf();
        builder.RegisterEntryPoint<GameTest>();
        builder.RegisterEntryPoint<WavePresenter>();
    }
}
