using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance; // Biến tĩnh để lưu instance của PlayerHealth

    [Header("Health Bar References")]
    public HealthBar heathBar; // Thanh máu cũ
    public HealthBar healthBar; // Thanh máu mới với animation - Kéo HealthBar component vào đây

    [Header("Health Settings")]
    public int maxHealth = 1000;
    public int currentHealth;
    public TextMeshProUGUI healthText;

    [Header("Combat Settings")]
    public float safeTime = 1f;
    private float safeTimeCoolDown;

    [Header("UI Settings")]
    public GameObject damageTextPrefab;
    public Canvas gameCanvas;

    // Private components
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyAI[] enemies = null;
    public bool isDead = false;

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
        // Tìm enemies
        enemies = FindObjectsOfType<EnemyAI>();
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning("No enemies found in the scene.");
        }
        else
        {
            Debug.Log("Enemies found: " + enemies.Length);
        }

        // Khởi tạo health
        currentHealth = maxHealth;

        // Tự động tìm HealthBar nếu chưa được gán
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();
        }

        // Cập nhật cả hai thanh máu
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        UpdateHealthBar(); // Cho thanh máu mới

        // Lấy components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

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

    // Method để cập nhật health bar mới
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    // Hàm này sẽ được gọi bởi kẻ địch - Đã được cập nhật với hiệu ứng
    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        // Kiểm tra thời gian an toàn
        if (safeTimeCoolDown > 0f)
        {
            if (damageTextPrefab != null && gameCanvas != null)
            {
                DamagePopup.Create(transform.position, 0, false, damageTextPrefab, gameCanvas);
            }
            return;
        }

        int oldHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        // Hiển thị damage popup
        if (damageTextPrefab != null && gameCanvas != null)
        {
            DamagePopup.Create(transform.position, damage, false, damageTextPrefab, gameCanvas);
        }

        // Cập nhật thanh máu cũ
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Hiệu ứng flash khi bị damage cho thanh máu mới
        if (healthBar != null)
        {
            healthBar.FlashHealthBar(Color.red, 0.2f);
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        UpdateHealthText();
        safeTimeCoolDown = safeTime; // Đặt lại thời gian an toàn

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHurt");
        }
    }

    // Method để reset health về giá trị mới
    public void ResetHealth(int newMaxHealth)
    {
        int oldHealth = currentHealth;
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;

        // Cập nhật thanh máu cũ
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Animate health change nếu có sự thay đổi lớn
        if (healthBar != null)
        {
            if (Mathf.Abs(newMaxHealth - oldHealth) > 100)
            {
                healthBar.AnimateHealthChange(oldHealth, currentHealth, maxHealth, 1f);
            }
            else
            {
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }
        }

        UpdateHealthText();
        Debug.Log($"Health reset to {currentHealth}/{maxHealth}");
    }

    // Method để reset health về max health hiện tại
    public void ResetToMaxHealth()
    {
        int oldHealth = currentHealth;
        currentHealth = maxHealth;

        // Cập nhật thanh máu cũ
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (healthBar != null)
        {
            healthBar.AnimateHealthChange(oldHealth, currentHealth, maxHealth, 0.5f);
        }

        UpdateHealthText();
        Debug.Log($"Health reset to max: {currentHealth}/{maxHealth}");
    }

    // Method để set health về giá trị cụ thể
    public void SetHealth(int newHealth)
    {
        int oldHealth = currentHealth;
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

        // Cập nhật thanh máu cũ
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        UpdateHealthText();
        Debug.Log($"Health set to: {currentHealth}/{maxHealth}");
    }

    // Method để heal player
    public void Heal(int healAmount)
    {
        int oldHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);

        // Cập nhật thanh máu cũ
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (healthBar != null)
        {
            healthBar.AnimateHealthChange(oldHealth, currentHealth, maxHealth, 0.3f);
        }

        UpdateHealthText();
        Debug.Log($"Player healed by {healAmount}. Current health: {currentHealth}/{maxHealth}");
    }

    // Method để tăng max health
    public void IncreaseMaxHealth(int increaseAmount)
    {
        int oldMaxHealth = maxHealth;
        maxHealth += increaseAmount;
        currentHealth += increaseAmount; // Cũng tăng current health

        // Cập nhật thanh máu cũ
        if (heathBar != null)
        {
            heathBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (healthBar != null)
        {
            healthBar.AnimateHealthChange(currentHealth - increaseAmount, currentHealth, maxHealth, 0.5f);
        }

        UpdateHealthText();
        Debug.Log($"Max health increased by {increaseAmount}. New health: {currentHealth}/{maxHealth}");
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


        Loading.TargetSceneName = "EndGame";
        SceneManager.LoadScene("LoadingScene");
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            // Hiển thị text theo dạng "Health: 1000"
            healthText.text = "Health: " + currentHealth;
        }
    }

    // ========== GETTER METHODS ==========

    // Method để lấy thông tin health hiện tại
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    // Method để kiểm tra xem player có còn sống không
    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    // Method để kiểm tra xem health có đầy không
    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }

    // ========== HEALTH BAR STYLING METHODS ==========

    // Method để set màu thanh máu tùy chỉnh
    public void SetHealthBarColor(Color color)
    {
        if (healthBar != null)
        {
            healthBar.SetHealthColor(color);
        }
    }

    // Method để reset màu thanh máu về mặc định
    public void ResetHealthBarColor()
    {
        if (healthBar != null)
        {
            healthBar.ResetHealthColor();
        }
    }
}