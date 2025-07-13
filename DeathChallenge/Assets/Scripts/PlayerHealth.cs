using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance; // Biến tĩnh để lưu instance của PlayerHealth

    [Header("HealthBar Settings")]
    public HeathBar heathBar;
    public int maxHealth = 1000;
    public int currentHealth;
    public TextMeshProUGUI healthText;

    private Rigidbody2D rb;
    private Animator animator;
    EnemyAI[] enemies = null;
    public bool isDead = false;


    [Header("UI Settings")]
    public GameObject damageTextPrefab;
    public Canvas gameCanvas;

    public float safeTime = 1f;
    private float safeTimeCoolDown;


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

        currentHealth = maxHealth;
        heathBar.UpdateHealthBar(currentHealth, maxHealth);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D

        UpdateHealthText();
    }

    void Update()
    {
        // Giảm thời gian an toàn nếu nó lớn hơn 0
        if (safeTimeCoolDown > 0f)
        {
            safeTimeCoolDown -= Time.deltaTime;
        }
    }

    // Hàm này sẽ được gọi bởi kẻ địch
    public void TakeDamage(int damage)
    {

        if (isDead)
        {
            return; 
        }

        // Kiểm tra thời gian an toàn
        if (safeTimeCoolDown > 0f)
        {
            DamagePopup.Create(transform.position, 0, false, damageTextPrefab, gameCanvas);
            return; 
        }

        currentHealth -= damage;

        if (damageTextPrefab != null && gameCanvas != null)
        {
            DamagePopup.Create(transform.position, damage, false, damageTextPrefab, gameCanvas);
        }

        heathBar.UpdateHealthBar(currentHealth, maxHealth);
        UpdateHealthText();
        safeTimeCoolDown = safeTime; // Đặt lại thời gian an toàn
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