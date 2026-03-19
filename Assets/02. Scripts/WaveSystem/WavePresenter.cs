using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class WavePresenter : IInitializable, IDisposable
{
    private readonly StageConfig _stageConfig;
    private readonly HeroModel _heroModel;
    private readonly WaveModel _waveModel;
    
    private CompositeDisposable _disposables = new CompositeDisposable();

    public WavePresenter(StageConfig stageConfig, HeroModel heroModel, WaveModel waveModel)
    {
        _stageConfig = stageConfig;
        _heroModel = heroModel;
        _waveModel = waveModel;
    }
    
    public void Initialize()
    {
        _heroModel.IsDead
            .Where(x => x && !_waveModel.IsGameOver.Value)
            .Subscribe(_ => HandleDefeat())
            .AddTo(_disposables);

        _waveModel.AliveEnemiesCount
            .Where(count => count == 0 && _waveModel.CurrentWaveIndex.Value >= _stageConfig.Waves.Count &&
                            !_waveModel.IsGameOver.Value)
            .Subscribe(_ => HandleVictory())
            .AddTo(_disposables);
    }

    private void HandleDefeat()
    {
        _waveModel.IsGameOver.Value = true;
        Debug.Log("게임 오버");
    }

    private void HandleVictory()
    {
        _waveModel.IsGameOver.Value = true;
        Debug.Log("게임 승리");
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}
