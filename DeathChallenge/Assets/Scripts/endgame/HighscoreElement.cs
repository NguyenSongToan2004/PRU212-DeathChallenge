using UnityEngine;
using System;

[Serializable]
public class HighscoreElement
{
    public string playerName;
    public int duration; // in seconds

    public HighscoreElement(string name, int duration)
    {
        playerName = name;
        this.duration = duration;
    }

}

