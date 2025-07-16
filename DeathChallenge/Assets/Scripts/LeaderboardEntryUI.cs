using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardEntryUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI dateText;
    public Image backgroundImage;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    public void Setup(int rank, LeaderboardEntry entry)
    {
        rankText.text = rank.ToString();
        playerNameText.text = entry.playerName;
        gameTimeText.text = FormatTime(entry.gameTime);
        dateText.text = entry.date.ToString("MM/dd/yyyy");

        // Highlight top 3
        if (rank <= 3)
        {
            backgroundImage.color = highlightColor;
        }
        else
        {
            backgroundImage.color = normalColor;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}