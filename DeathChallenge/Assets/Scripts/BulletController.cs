using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 50; 

    private Rigidbody2D rb;

    void Awake()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the projectile.");
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Tự hủy sau 5 giây nếu không có va chạm
        Destroy(gameObject, 5f);
    }

    public void Fire(Vector3 direction)
    {
        //rb.linearVelocity = direction.normalized * speed;
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the player!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }else
        {
            Debug.Log("Projectile hit something else: " + other.name);
        }
    }
}