using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseManager : MonoBehaviour
{
    public GameObject pausePanel;  // Gắn panel trong Inspector
    private bool isPaused = false;

    private float lastToggleTime = 0f;
    private float toggleCooldown = 0.3f; 

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
            lastToggleTime = Time.unscaledTime;
        }

        Debug.Log("Game is now " + (isPaused ? "Paused" : "Running"));
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed && Time.unscaledTime - lastToggleTime > toggleCooldown)
        {
            TogglePause();
        }
    }
}
