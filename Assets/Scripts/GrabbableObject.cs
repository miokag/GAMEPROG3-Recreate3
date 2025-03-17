using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        EnemyManager enemy = collision.collider.GetComponent<EnemyManager>();
        if (enemy != null)
        {
            enemy.TakeDamage();
        }
    }
}