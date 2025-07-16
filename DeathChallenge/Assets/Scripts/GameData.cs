using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance; // Biến static để truy cập từ bất cứ đâu

    public int selectedCharacterIndex = 0; // Lưu chỉ số nhân vật được chọn

    private void Awake()
    {
        // Logic Singleton kinh điển
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}