using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LobbyLifetimeScope : LifetimeScope
{
    [Header("View")]
    [SerializeField] private LobbyView lobbyView;
    
    [Header("Stage Datas")]
    [SerializeField] private List<StageConfig> stageConfigs; 
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(lobbyView);
        builder.RegisterInstance<IReadOnlyList<StageConfig>>(stageConfigs);
        builder.RegisterEntryPoint<LobbyPresenter>();
        
    }
}
