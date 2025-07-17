using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI playTimeText; // Tham chiếu đến TextMeshProUGUI để hiển thị thời gian chơi

    private void Awake()
    {
        // Kiểm tra và khởi tạo GameStats nếu chưa có
        if (GameData.playTime == 0f)
        {
            GameData.playTime = 0f;
        }
    }

    private void Update()
    {
        GameData.playTime += Time.deltaTime;

        if (playTimeText != null)
        {
            int minutes = Mathf.FloorToInt(GameData.playTime / 60f);
            int seconds = Mathf.FloorToInt(GameData.playTime % 60f);

            playTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogWarning("PlayTimeText is not assigned in the inspector.");
        }
    }
}
