using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance; // Biến tĩnh để lưu instance của PlayerHealth
    public int maxHealth = 1000;
    public int currentHealth;
    public TextMeshProUGUI healthText;
    private Rigidbody2D rb;
    private Animator animator;
    EnemyAI[] enemies = null;
    public bool isDead = false; // Biến để đánh dấu người chơi đã chết

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // tránh trùng
        }
    }

    void Start()
    {
        enemies = FindObjectsOfType<EnemyAI>();
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found in the scene.");
        }
        else
        {
            Debug.Log("Enemies found: " + enemies.Length);
        }
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D

        UpdateHealthText();
    }

    // Hàm này sẽ được gọi bởi kẻ địch
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player's Health: " + currentHealth); // Log ra máu hiện tại
        UpdateHealthText();
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHurt");
        }
    }

    // Hàm nhận lực đẩy lùi
    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            // Dùng Impulse để tạo một cú đẩy tức thời
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        isDead = true;
        animator.SetTrigger("isDead");

        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.OnPlayerDied();
            }
        }

    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            // Hiển thị text theo dạng "Health: 1000"
            healthText.text = "Health: " + currentHealth;
        }
    }

}