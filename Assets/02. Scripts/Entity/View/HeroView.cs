using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    [SerializeField] private float gizmoRange = 1f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, gizmoRange);
    }
    
    public void SetGizmoRange(float range) => gizmoRange = range;
}
