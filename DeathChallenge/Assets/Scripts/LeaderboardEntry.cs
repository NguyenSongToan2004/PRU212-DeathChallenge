using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public float gameTime;
    public bool isWin;
    public System.DateTime date;

    public LeaderboardEntry(string name, float time, bool win)
    {
        playerName = name;
        gameTime = time;
        isWin = win;
        date = System.DateTime.Now;
    }
}
