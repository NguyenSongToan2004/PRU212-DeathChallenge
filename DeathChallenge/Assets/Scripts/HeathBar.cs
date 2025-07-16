using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Image fillBar; // Kéo Image của thanh máu vào đây
    public TextMeshProUGUI valueText; // Kéo Text của thanh máu vào đây

    [Header("Animation Settings")]
    public bool useSmoothing = true; // Có sử dụng hiệu ứng smooth không
    public float smoothSpeed = 2f; // Tốc độ smooth animation

    [Header("Color Settings")]
    public bool useColorChange = true; // Có đổi màu theo mức máu không
    public Color highHealthColor = Color.green; // Màu khi máu cao
    public Color mediumHealthColor = Color.yellow; // Màu khi máu trung bình
    public Color lowHealthColor = Color.red; // Màu khi máu thấp

    [Header("Thresholds")]
    [Range(0f, 1f)]
    public float mediumHealthThreshold = 0.6f; // Ngưỡng máu trung bình
    [Range(0f, 1f)]
    public float lowHealthThreshold = 0.3f; // Ngưỡng máu thấp

    // Private variables
    private float targetFillAmount = 1f;
    private int currentDisplayHealth;
    private int maxDisplayHealth;
    private bool isAnimating = false;

    void Start()
    {
        // Khởi tạo giá trị mặc định
        if (fillBar != null)
        {
            fillBar.fillAmount = 1f;
            targetFillAmount = 1f;
        }

        // Tự động tìm PlayerHealth nếu cần
        if (currentDisplayHealth == 0)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                UpdateHealthBar(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
            }
        }
    }

    void Update()
    {
        // Smooth animation cho thanh máu
        if (useSmoothing && fillBar != null)
        {
            if (Mathf.Abs(fillBar.fillAmount - targetFillAmount) > 0.001f)
            {
                fillBar.fillAmount = Mathf.Lerp(fillBar.fillAmount, targetFillAmount, smoothSpeed * Time.deltaTime);

                // Cập nhật màu sắc trong quá trình animation
                if (useColorChange)
                {
                    UpdateHealthColor(fillBar.fillAmount);
                }
            }
        }
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        // Đảm bảo giá trị không âm
        currentHealth = Mathf.Max(0, currentHealth);
        maxHealth = Mathf.Max(1, maxHealth);

        // Lưu giá trị hiển thị
        currentDisplayHealth = currentHealth;
        maxDisplayHealth = maxHealth;

        // Tính toán fill amount
        float fillAmount = (float)currentHealth / maxHealth;

        if (useSmoothing)
        {
            targetFillAmount = fillAmount;
        }
        else
        {
            if (fillBar != null)
            {
                fillBar.fillAmount = fillAmount;
            }
        }

        // Cập nhật text
        UpdateHealthText();

        // Cập nhật màu sắc
        if (useColorChange)
        {
            UpdateHealthColor(fillAmount);
        }
    }

    private void UpdateHealthText()
    {
        if (valueText != null)
        {
            valueText.text = $"{currentDisplayHealth} / {maxDisplayHealth}";
        }
    }

    private void UpdateHealthColor(float healthPercentage)
    {
        if (fillBar == null || !useColorChange) return;

        Color targetColor;

        if (healthPercentage > mediumHealthThreshold)
        {
            // Máu cao - màu xanh lá
            targetColor = highHealthColor;
        }
        else if (healthPercentage > lowHealthThreshold)
        {
            // Máu trung bình - màu vàng
            targetColor = mediumHealthColor;
        }
        else
        {
            // Máu thấp - màu đỏ
            targetColor = lowHealthColor;
        }

        fillBar.color = targetColor;
    }

    // Method để set màu cụ thể
    public void SetHealthColor(Color color)
    {
        if (fillBar != null)
        {
            fillBar.color = color;
        }
    }

    // Method để reset về màu mặc định
    public void ResetHealthColor()
    {
        if (fillBar != null)
        {
            UpdateHealthColor(fillBar.fillAmount);
        }
    }

    // Method để animate thanh máu với hiệu ứng đặc biệt
    public void AnimateHealthChange(int fromHealth, int toHealth, int maxHealth, float duration = 1f)
    {
        if (isAnimating) return;

        StartCoroutine(AnimateHealthCoroutine(fromHealth, toHealth, maxHealth, duration));
    }

    private IEnumerator AnimateHealthCoroutine(int fromHealth, int toHealth, int maxHealth, float duration)
    {
        isAnimating = true;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // Smooth curve
            progress = Mathf.SmoothStep(0f, 1f, progress);

            // Interpolate health value
            int currentHealth = Mathf.RoundToInt(Mathf.Lerp(fromHealth, toHealth, progress));

            // Update immediately without smoothing
            bool oldSmoothing = useSmoothing;
            useSmoothing = false;
            UpdateHealthBar(currentHealth, maxHealth);
            useSmoothing = oldSmoothing;

            yield return null;
        }

        // Ensure final value is set
        UpdateHealthBar(toHealth, maxHealth);
        isAnimating = false;
    }

    // Method để làm nháy thanh máu (khi bị damage)
    public void FlashHealthBar(Color flashColor, float flashDuration = 0.2f)
    {
        StartCoroutine(FlashCoroutine(flashColor, flashDuration));
    }

    private IEnumerator FlashCoroutine(Color flashColor, float duration)
    {
        if (fillBar == null) yield break;

        Color originalColor = fillBar.color;
        fillBar.color = flashColor;

        yield return new WaitForSeconds(duration);

        fillBar.color = originalColor;
    }

    // Method để ẩn/hiện thanh máu
    public void SetHealthBarVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    // Method để lấy thông tin hiện tại
    public float GetCurrentFillAmount()
    {
        return fillBar != null ? fillBar.fillAmount : 0f;
    }

    public int GetCurrentDisplayHealth()
    {
        return currentDisplayHealth;
    }

    public int GetMaxDisplayHealth()
    {
        return maxDisplayHealth;
    }

    // Method để set style cho thanh máu
    public void SetHealthBarStyle(bool smoothing, bool colorChange, float speed = 2f)
    {
        useSmoothing = smoothing;
        useColorChange = colorChange;
        smoothSpeed = speed;
    }
}