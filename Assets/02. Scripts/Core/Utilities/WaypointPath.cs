using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaypointPath : MonoBehaviour
{
    [Header("Path Color")]
    public Color pathColor;
    
    [Header("Waypoint Path")]
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();

    public List<Vector3> GetPathPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (var waypoint in waypoints)
        {
            if (waypoint != null) positions.Add(waypoint.position);
        }
        return positions;
    }
    
#if UNITY_EDITOR
    [ContextMenu("Save Path to SO")]
    private void SavePathToSO()
    {
        string savePath = EditorUtility.SaveFilePanelInProject(
            "경로 데이터 저장", 
            "NewPathData", 
            "asset", 
            "경로 정보를 저장할 위치를 선택하세요.");

        if (string.IsNullOrEmpty(savePath)) return; 

        PathDataSO newPathSO = ScriptableObject.CreateInstance<PathDataSO>();
        newPathSO.PathPositions = GetPathPositions();

        AssetDatabase.CreateAsset(newPathSO, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"<color=green>경로 데이터가 성공적으로 추출되었습니다!</color>\n경로: {savePath}");
    }
#endif
    
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = pathColor;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);
            }
        }
        
        if (waypoints[waypoints.Count - 1] != null)
            Gizmos.DrawSphere(waypoints[waypoints.Count - 1].position, 0.2f);
    }
}
