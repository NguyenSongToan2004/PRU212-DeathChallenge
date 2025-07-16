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
        // Cập nhật TextMeshProUGUI với thời gian chơi
        if (playTimeText != null)
        {
            playTimeText.text = "Play Time: " + GameData.playTime.ToString("F0");
        }
        else
        {
            Debug.LogWarning("PlayTimeText is not assigned in the inspector.");
        }
        //Debug.Log("Play time: " + GameStats.playTime);
    }
}
