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

        // Disable gravity initially (enemy is standing/moving)
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true; 
        }
    }

    void Update()
    {
        if (isDowned) return;

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Check if the enemy is close enough to punch the player
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

            if (Vector3.Distance(transform.position, player.position) <= punchRange)
            {
                KillPlayer();
            }
        }
    }

    void KillPlayer()
    {
        Debug.Log("Player died! Resetting scene...");
        // Reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage(Vector3 impactForce)
    {
        health--;
        if (health <= 0)
        {
            DownEnemy(impactForce);
        }
    }

    void DownEnemy(Vector3 impactForce)
    {
        Debug.Log("Enemy downed!");
        isDowned = true;

        moveSpeed = 0f;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddForce(impactForce, ForceMode.Impulse);
        }

        Invoke("DestroyEnemy", 2f);
    }

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, punchRange);
    }
}