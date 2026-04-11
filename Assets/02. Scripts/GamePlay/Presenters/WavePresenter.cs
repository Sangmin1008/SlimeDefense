using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer.Unity;

public class WavePresenter : IInitializable, IDisposable
{
    private readonly StageConfig _stageConfig;
    private readonly CommanderModel _commanderModel;
    private readonly WaveModel _waveModel;
    
    private CompositeDisposable _disposables = new CompositeDisposable();

    public WavePresenter(StageConfig stageConfig, CommanderModel commanderModel, WaveModel waveModel)
    {
        _stageConfig = stageConfig;
        _commanderModel = commanderModel;
        _waveModel = waveModel;
    }
    
    public void Initialize()
    {
        Debug.Log("Initializing WavePresenter");
        _commanderModel.IsDead
            .Where(x => x && !_waveModel.IsGameOver.Value)
            .Subscribe(_ => HandleDefeat())
            .AddTo(_disposables);

        _waveModel.AliveEnemiesCount
            .Where(count => count == 0 && _waveModel.CurrentWaveIndex.Value >= _stageConfig.Waves.Count - 1 &&
                            !_waveModel.IsGameOver.Value)
            .Subscribe(_ => HandleVictory())
            .AddTo(_disposables);
    }

    private void HandleDefeat()
    {
        _waveModel.IsDefeat.Value = true;
        _waveModel.IsGameOver.Value = true;
        Debug.Log("게임 오버");
    }

    private void HandleVictory()
    {
        _waveModel.IsVictory.Value = true;
        _waveModel.IsGameOver.Value = true;
        Debug.Log("게임 승리");
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}
