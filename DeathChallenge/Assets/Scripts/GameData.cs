using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static float playTime = 0f; // Thời gian chơi game

    public static int selectedCharacterIndex = 0;


    // Danh sách các map đã chơi qua (ví dụ: "Level1", "DesertMap", v.v.)
    public static List<string> playedMaps = new List<string>();

    public static void AddPlayedMap(string mapName)
    {
        if (!playedMaps.Contains(mapName))
        {
            playedMaps.Add(mapName);
            Debug.Log("Map đã thêm: " + mapName);
        }
        else
        {
            Debug.Log("Map đã được chơi trước đó: " + mapName);
        }
    }

}