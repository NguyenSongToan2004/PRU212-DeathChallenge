using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float disappearTimer;
    private Color textColor;
    private Color originalColor; // Lưu màu gốc

    public float moveSpeed = 2f;
    public float disappearSpeed = 3f;

    // Hàm này được gọi để thiết lập số sát thương
    public void Setup(int damageAmount, bool isCritical)
    {
        textMesh.SetText(damageAmount.ToString());

        if (isCritical)
        {
            // Nếu là chí mạng: chữ to hơn và màu vàng
            textMesh.fontSize *= 1.5f; // Tăng 50% cỡ chữ
            textMesh.color = Color.yellow;
        }
        else
        {
            // Nếu là đòn thường: dùng màu gốc
            textMesh.color = originalColor;
        }
    }

    //public void Setup(int damageAmount)
    //{
    //    textMesh.SetText(damageAmount.ToString());
    //    textMesh.color = originalColor;
    //}

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalColor = textMesh.color; // Lưu lại màu gốc khi bắt 
        textColor = textMesh.color;
        disappearTimer = 1f;
    }

    private void Update()
    {
        // Di chuyển số lên trên
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;

        // Mờ dần
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // HÀM TĨNH (STATIC) ĐỂ TẠO POPUP TỪ BẤT CỨ ĐÂU
    public static void Create(Vector3 position, int damage, bool isCritical, GameObject damageTextPrefab, Canvas gameCanvas)
    {
        // 1. Tạo popup làm con của Canvas
        GameObject popupObj = Instantiate(damageTextPrefab, gameCanvas.transform);

        // 2. Thêm một chút ngẫu nhiên vào vị trí để các số không đè lên nhau
        float xOffset = Random.Range(-1f, 1f);
        float yOffset = Random.Range(1f, 2f);
        Vector3 spawnPosition = position + new Vector3(xOffset, yOffset, 0);

        // 3. Chuyển đổi vị trí thế giới thành vị trí trên màn hình (UI)
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(spawnPosition);
        popupObj.transform.position = screenPosition;

        // 4. Lấy script và gọi hàm Setup
        DamagePopup popupScript = popupObj.GetComponent<DamagePopup>();
        popupScript.Setup(damage, isCritical);
    }
}