using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModel
{
    public int Damage { get; }
    public float Speed { get; }
    public float MaxDistance { get; }
    
    public Vector3 CurrentDirection { get; set; }
    public float TraveledDistance { get; set; }

    public EnemyModel TargetModel { get; }
    public GameObject TargetView { get; }
    
    public ProjectileModel(int damage, float speed, float maxDistance, EnemyModel targetModel, GameObject targetView)
    {
        Damage = damage;
        Speed = speed;
        MaxDistance = maxDistance;
        TargetModel = targetModel;
        TargetView = targetView;
        TraveledDistance = 0f;
    }
}
