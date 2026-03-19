using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObject/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [Header("Wave Settings")]
    public EnemyConfig EnemyType;
    public int SpawnCount;
    public float SpawnInterval;
    public float DelayTimeBeforeWave;
    
    [Header("Wave Path")]
    public PathDataSO PathData;
}
