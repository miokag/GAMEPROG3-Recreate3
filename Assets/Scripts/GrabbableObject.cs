using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public float impactForce = 10f;

    void OnCollisionEnter(Collision collision)
    {
        EnemyManager enemy = collision.collider.GetComponent<EnemyManager>();
        if (enemy != null)
        {
            Vector3 impactDirection = collision.contacts[0].point - transform.position;
            impactDirection.Normalize();

            enemy.TakeDamage(impactDirection * impactForce);
        }
    }
}