using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    public event Action<EnemyView> OnHitEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyView enemy))
        {
            OnHitEnemy?.Invoke(enemy);
        }
    }

    public void Move(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
