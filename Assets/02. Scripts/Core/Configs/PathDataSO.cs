using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathData", menuName = "ScriptableObject/PathData")]
public class PathDataSO : ScriptableObject
{
    [Header("Path Data")]
    public List<Vector3> PathPositions = new List<Vector3>();
}
