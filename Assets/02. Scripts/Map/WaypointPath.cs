using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
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
    
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.red;
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
