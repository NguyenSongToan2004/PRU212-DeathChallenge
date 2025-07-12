using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningController : MonoBehaviour
{
    // Kéo Panel chiến thắng của bạn vào đây trong Inspector
    public GameObject winningPanel;

    void Start()
    {
        // Ẩn panel đi khi game bắt đầu
        if (winningPanel != null)
        {
            winningPanel.SetActive(false);
        }
    }

    // Hàm này sẽ tự động được gọi khi có một đối tượng khác đi vào trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có tag là "Player" không
        if (other.CompareTag("Player"))
        {
            // Nếu đúng là Player, hiển thị màn hình chiến thắng
            Debug.Log("Player has reached the goal!");
            ShowWinningScreen();
        }
    }

    // Gọi hàm này khi người chơi chiến thắng để hiển thị màn hình
    public void ShowWinningScreen()
    {
        if (winningPanel != null)
        {
            winningPanel.SetActive(true);
        }
    }

    // Gán hàm này vào sự kiện OnClick của nút Reset
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}