using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    private Camera mainCamera;
    private float playerWidth, playerHeight;

    void Start()
    {
        // Lấy camera chính một lần để tối ưu
        mainCamera = Camera.main;

        // Tính toán kích thước của nhân vật
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            playerWidth = spriteRenderer.bounds.size.x / 2;
            playerHeight = spriteRenderer.bounds.size.y / 2;
        }
        else
        {
            var collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                playerWidth = collider.bounds.extents.x;
                playerHeight = collider.bounds.extents.y;
            }
        }
    }

    void LateUpdate()
    {
        // --- PHẦN ĐƯỢC DI CHUYỂN VÀO LATEUPDATE ---
        // Chuyển đổi tọa độ của các góc màn hình sang tọa độ thế giới (world point) mỗi frame
        Vector2 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector2 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        // Xác định các giới hạn dựa trên vị trí camera hiện tại
        float xMin = bottomLeft.x + playerWidth;
        float xMax = topRight.x - playerWidth;

        float yMin = bottomLeft.y + playerHeight;
        float yMax = topRight.y - playerHeight;
        // --- KẾT THÚC PHẦN DI CHUYỂN ---

        // Lấy vị trí hiện tại của nhân vật
        Vector3 currentPosition = transform.position;

        // "Kẹp" vị trí của nhân vật trong các giới hạn đã được cập nhật
        float clampedX = Mathf.Clamp(currentPosition.x, xMin, xMax);
        float clampedY = Mathf.Clamp(currentPosition.y, yMin, yMax);

        // Cập nhật lại vị trí cho nhân vật
        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }
}