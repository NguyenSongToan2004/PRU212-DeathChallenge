﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinningController : MonoBehaviour
{
    [Header("UI References")]
    // Kéo Panel chiến thắng của bạn vào đây trong Inspector
    public GameObject winningPanel;

    [Header("Player References")]
    // Thêm reference đến PlayerHealth
    public PlayerHealth[] playerHealth;

    [Header("Scene Transition Settings")]
    // Thời gian delay trước khi chuyển scene (để player có thể thấy winning panel)
    public float delayBeforeNextScene = 2f;

    // Tên scene tiếp theo
    public string nextSceneName;

    [Header("Health Reset Settings")]
    // Giá trị health sẽ được reset về
    public int resetHealthValue = 1000;
    private PlayerHealth currentHeath;

    void Start()
    {
        // Ẩn panel đi khi game bắt đầu
        if (winningPanel != null)
        {
            winningPanel.SetActive(false);
        }

        Debug.Log("selected  " + GameData.selectedCharacterIndex);

        currentHeath = playerHealth.Length > 0 ? playerHealth[GameData.selectedCharacterIndex] : null;

        // Tự động tìm PlayerHealth nếu chưa được gán
        if (playerHealth == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                currentHeath = playerObject.GetComponent<PlayerHealth>();
            }
        }

        // Kiểm tra nếu vẫn không tìm thấy PlayerHealth
        if (playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth not found! Please assign it in the Inspector.");
        }
    }

    // Hàm này sẽ tự động được gọi khi có một đối tượng khác đi vào trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có tag là "Player" không
        if (other.CompareTag("Player"))
        {
            // Nếu đúng là Player, hiển thị màn hình chiến thắng
            Debug.Log("Player has reached the goal!");
            ShowWinningScreen();
        }
    }

    // Gọi hàm này khi người chơi chiến thắng để hiển thị màn hình
    public void ShowWinningScreen()
    {
        // Dừng nhạc nền hiện tại
        AudioManager.instance.StopBackground("Background_1");

        // Phát âm thanh chiến thắng
        AudioManager.instance.Play("Victory");

        // Hiển thị winning panel
        if (winningPanel != null)
        {
            winningPanel.SetActive(true);
        }

        ResetPlayerHealth();
        // Loading.SetTargetScene("EndGame");
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    // Reset health của player về giá trị đã định
    private void ResetPlayerHealth()
    {
        if (playerHealth != null)
        {
            currentHeath.ResetHealth(resetHealthValue);
            Debug.Log($"Player health reset to {resetHealthValue}");
        }
        else
        {
            Debug.LogWarning("PlayerHealth reference not found! Cannot reset health.");
        }
    }

    // Coroutine để chuyển scene sau một khoảng thời gian
    private IEnumerator LoadNextSceneAfterDelay()
    {
        Debug.Log($"Waiting {delayBeforeNextScene} seconds before loading next scene...");
        yield return new WaitForSeconds(delayBeforeNextScene);

        // Chuyển đến scene tiếp theo
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Loading scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Nếu không có tên scene, load scene tiếp theo theo index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Kiểm tra xem có scene tiếp theo không
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log($"Loading next scene with index: {nextSceneIndex}");
                Loading.TargetSceneName = SceneManager.GetSceneByBuildIndex(nextSceneIndex).name;
                SceneManager.LoadScene("LoadingScene");
            }
            else
            {
                Debug.LogWarning("No next scene found! Please set nextSceneName or add more scenes to Build Settings.");
            }
        }
    }

    // Hàm để chuyển scene ngay lập tức (có thể gọi từ button)
    public void LoadNextSceneImmediately()
    {
        ResetPlayerHealth();

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    // Gán hàm này vào sự kiện OnClick của nút Reset (nếu vẫn cần)
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hàm để restart từ scene đầu tiên
    public void RestartFromBeginning()
    {
        SceneManager.LoadScene(0);
    }

    // Hàm để thoát game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
