using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static float playTime = 0f; // Thời gian chơi game

    public static int selectedCharacterIndex = 0;

    public static bool isLose = false; // Biến để kiểm tra trạng thái thua

    // NEW: Lưu danh sách các map đã đi qua
    private static readonly List<string> visitedMaps = new();

    // NEW: Thêm map nếu chưa có
    public static void AddVisitedMap(string sceneName)
    {
        if (!visitedMaps.Contains(sceneName))
        {
            visitedMaps.Add(sceneName);
        }
    }

    // NEW: Lấy danh sách các map đã đi qua
    public static List<string> GetVisitedMaps()
    {
        return new List<string>(visitedMaps);
    }

    // Optional: Xóa/reset danh sách map
    public static void ResetVisitedMaps()
    {
        visitedMaps.Clear();
    }
}