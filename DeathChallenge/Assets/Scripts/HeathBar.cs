using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    public Image fillBar; // Kéo Image của thanh máu vào đây
    public TextMeshProUGUI valueText; // Kéo Text của thanh máu vào đây

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        // Cập nhật thanh máu
        float fillAmount = (float)currentHealth / maxHealth;
        fillBar.fillAmount = fillAmount;
        // Cập nhật giá trị hiển thị
        valueText.text = $"{currentHealth} / {maxHealth}";
    }
}
