using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3.5f;
    public float punchRange = 2f; 
    public float punchCooldown = 2f; 
    public int health = 1;

    private float lastPunchTime;
    private bool isDowned = false; 
    private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isDowned) return;

        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) <= punchRange)
        {
            PunchPlayer();
        }
    }

    void PunchPlayer()
    {
        if (Time.time - lastPunchTime >= punchCooldown)
        {
            Debug.Log("Enemy punched the player!");
            lastPunchTime = Time.time;

            // Check if the punch hits the player
            if (Vector3.Distance(transform.position, player.position) <= punchRange)
            {
                KillPlayer();
            }
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player died! Resetting scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            DownEnemy();
        }
    }

    void DownEnemy()
    {
        Debug.Log("Enemy downed!");
        isDowned = true; 

        moveSpeed = 0f;

        // Allow the enemy to fall realistically
        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.useGravity = true; 
        }

        // Freeze the Rigidbody after a short delay
        Invoke("FreezeRigidbody", 2f);
    }

    void FreezeRigidbody()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero; 
            rb.isKinematic = true; 
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the punch range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, punchRange);
    }
}