using UnityEngine;

public class SceneBackgroundMusic : MonoBehaviour
{
    void Start()
    {
        // Phát nhạc nền khi scene bắt đầu
        AudioManager.instance.PlayBackground("Background_1");
    }
}