using UnityEngine;

public class SceneBackgroundMusic : MonoBehaviour
{
    void Start()
    {
        // Tìm AudioManager của scene hiện tại để lấy sounds
        AudioManager[] allAudioManagers = FindObjectsOfType<AudioManager>();
        foreach (AudioManager am in allAudioManagers)
        {
            if (am != AudioManager.instance)
            {
                // Reload sounds từ AudioManager của scene hiện tại
                AudioManager.instance.ReloadSounds(am.sounds);
                Destroy(am.gameObject);
                break;
            }
        }
        
        // Phát nhạc nền
        AudioManager.instance.PlayBackground("Background_1");
    }
}
