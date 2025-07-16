using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject winEndGamePanel;
    public GameObject loseEndGamePanel;

    [Header("Win Panel Elements")]
    public TextMeshProUGUI winMessage;
    public TextMeshProUGUI loseMessage;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI leaderboardTitle;
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button settingsButton;
    public ParticleSystem hellParticles;
    public ParticleSystem overlayParticles;
    public ScrollRect leaderboardScrollView;
    public Transform leaderboardContent;

    [Header("Game Data")]
    public float gameTime;
    public string playerName;
    public bool isWin;

    [Header("Leaderboard")]
    public GameObject leaderboardEntryPrefab;
    public List<LeaderboardEntry> leaderboardData = new List<LeaderboardEntry>();

    private void Start()
    {
        SetupUI();
        DisplayResults();
        SetupButtons();
        LoadLeaderboard();
    }

    private void SetupUI()
    {
        // Hiển thị panel phù hợp
        winEndGamePanel.SetActive(isWin);
        loseEndGamePanel.SetActive(!isWin);

        // Cập nhật thông tin player
        playerNameText.text = playerName;
        gameTimeText.text = FormatTime(gameTime);

        // Cập nhật message
        if (isWin)
        {
            winMessage.text = "Your spirit has found its way to heaven.";
            overlayParticles.Play();
        }
        else
        {
            loseMessage.text = "YOUR SOUL IS DAMNED TO HELL...";
            hellParticles.Play();
        }
    }

    private void DisplayResults()
    {
        leaderboardTitle.text = isWin ? "Leaderboard - Heaven" : "Leaderboard - Hell";
    }

    private void SetupButtons()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    private void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OpenSettings()
    {
        // Mở settings panel
        Debug.Log("Opening Settings...");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void LoadLeaderboard()
    {
        // Xóa các entry cũ
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Tải dữ liệu leaderboard
        leaderboardData = LeaderboardManager.Instance.GetLeaderboard(isWin);

        // Thêm entry mới
        for (int i = 0; i < leaderboardData.Count; i++)
        {
            CreateLeaderboardEntry(leaderboardData[i], i + 1);
        }
    }

    private void CreateLeaderboardEntry(LeaderboardEntry entry, int rank)
    {
        GameObject entryObject = Instantiate(leaderboardEntryPrefab, leaderboardContent);
        LeaderboardEntryUI entryUI = entryObject.GetComponent<LeaderboardEntryUI>();
        entryUI.Setup(rank, entry);
    }


}
