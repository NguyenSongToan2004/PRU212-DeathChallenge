using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game Settings")]
    public string playerName = "Player";
    public float gameStartTime;

    private bool gameEnded = false;

    private void Start()
    {
        gameStartTime = Time.time;
    }

    public void EndGame(bool isWin)
    {
        if (gameEnded) return;

        gameEnded = true;
        float gameTime = Time.time - gameStartTime;

        // Lưu kết quả vào leaderboard
        LeaderboardManager.Instance.AddEntry(playerName, gameTime, isWin);

        // Chuyển đến màn hình EndGame
        var endGameManager = FindFirstObjectByType<EndGameManager>();
        if (endGameManager != null)
        {
            endGameManager.playerName = playerName;
            endGameManager.gameTime = gameTime;
            endGameManager.isWin = isWin;
            endGameManager.gameObject.SetActive(true);
        }
    }
}