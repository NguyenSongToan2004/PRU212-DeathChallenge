using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseManager : MonoBehaviour
{
    public GameObject pausePanel;  // Gắn panel trong Inspector
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }

        Debug.Log("Game is now " + (isPaused ? "Paused" : "Running"));
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            TogglePause();
        }
    }
}
