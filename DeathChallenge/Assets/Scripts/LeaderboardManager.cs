using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [Header("Leaderboard Settings")]
    public int maxEntries = 10;

    private List<LeaderboardEntry> winnerLeaderboard = new List<LeaderboardEntry>();
    private List<LeaderboardEntry> loserLeaderboard = new List<LeaderboardEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEntry(string playerName, float gameTime, bool isWin)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(playerName, gameTime, isWin);

        if (isWin)
        {
            winnerLeaderboard.Add(newEntry);
            winnerLeaderboard = winnerLeaderboard.OrderBy(x => x.gameTime).Take(maxEntries).ToList();
        }
        else
        {
            loserLeaderboard.Add(newEntry);
            loserLeaderboard = loserLeaderboard.OrderBy(x => x.gameTime).Take(maxEntries).ToList();
        }

        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetLeaderboard(bool isWin)
    {
        return isWin ? winnerLeaderboard : loserLeaderboard;
    }

    private void SaveLeaderboard()
    {
        // Lưu winner leaderboard
        for (int i = 0; i < winnerLeaderboard.Count; i++)
        {
            var entry = winnerLeaderboard[i];
            PlayerPrefs.SetString($"Winner_{i}_Name", entry.playerName);
            PlayerPrefs.SetFloat($"Winner_{i}_Time", entry.gameTime);
            PlayerPrefs.SetString($"Winner_{i}_Date", entry.date.ToBinary().ToString());
        }
        PlayerPrefs.SetInt("WinnerCount", winnerLeaderboard.Count);

        // Lưu loser leaderboard
        for (int i = 0; i < loserLeaderboard.Count; i++)
        {
            var entry = loserLeaderboard[i];
            PlayerPrefs.SetString($"Loser_{i}_Name", entry.playerName);
            PlayerPrefs.SetFloat($"Loser_{i}_Time", entry.gameTime);
            PlayerPrefs.SetString($"Loser_{i}_Date", entry.date.ToBinary().ToString());
        }
        PlayerPrefs.SetInt("LoserCount", loserLeaderboard.Count);

        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        // Load winner leaderboard
        int winnerCount = PlayerPrefs.GetInt("WinnerCount", 0);
        for (int i = 0; i < winnerCount; i++)
        {
            string name = PlayerPrefs.GetString($"Winner_{i}_Name");
            float time = PlayerPrefs.GetFloat($"Winner_{i}_Time");
            string dateString = PlayerPrefs.GetString($"Winner_{i}_Date");

            var entry = new LeaderboardEntry(name, time, true);
            if (long.TryParse(dateString, out long dateBinary))
            {
                entry.date = System.DateTime.FromBinary(dateBinary);
            }
            winnerLeaderboard.Add(entry);
        }

        // Load loser leaderboard
        int loserCount = PlayerPrefs.GetInt("LoserCount", 0);
        for (int i = 0; i < loserCount; i++)
        {
            string name = PlayerPrefs.GetString($"Loser_{i}_Name");
            float time = PlayerPrefs.GetFloat($"Loser_{i}_Time");
            string dateString = PlayerPrefs.GetString($"Loser_{i}_Date");

            var entry = new LeaderboardEntry(name, time, false);
            if (long.TryParse(dateString, out long dateBinary))
            {
                entry.date = System.DateTime.FromBinary(dateBinary);
            }
            loserLeaderboard.Add(entry);
        }
    }
}