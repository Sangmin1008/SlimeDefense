using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WaveModel
{
    public ReactiveProperty<int> CurrentWaveIndex { get; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> AliveEnemiesCount { get; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> TotalEnemiesInCurrentWave { get; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<bool> IsGameOver { get; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsVictory { get; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsDefeat { get; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<int> NextWaveDelayCountdown { get; } = new ReactiveProperty<int>(0);
}
