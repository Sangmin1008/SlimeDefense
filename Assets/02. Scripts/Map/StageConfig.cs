using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "ScriptableObject/StageConfig")]
public class StageConfig : ScriptableObject
{
    public List<WaveConfig> Waves;
}
