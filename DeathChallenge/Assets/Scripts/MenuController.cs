using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject selectedPlayerPreview; // Reference to the selected player preview
    // List player preview list
    [SerializeField] private GameObject[] playerPreviews;

    public void Start()
    {
        // Set the first player preview as selected by default
        if (playerPreviews.Length > 0)
        {
            selectedPlayerPreview = playerPreviews[0];
            selectedPlayerPreview.SetActive(true);
        }
    }

    public void NextPlayer()
    {
        // Find the index of the currently active player preview
        int currentIndex = System.Array.IndexOf(playerPreviews, selectedPlayerPreview);
        // Calculate the next index, wrapping around if necessary
        int nextIndex = (currentIndex + 1) % playerPreviews.Length;
        // Set the new selected player preview
        selectedPlayerPreview.SetActive(false);
        selectedPlayerPreview = playerPreviews[nextIndex];
        selectedPlayerPreview.SetActive(true);
    }

    public void PreviousPlayer()
    {
        // Find the index of the currently active player preview
        int currentIndex = System.Array.IndexOf(playerPreviews, selectedPlayerPreview);
        // Calculate the previous index, wrapping around if necessary
        int previousIndex = (currentIndex - 1 + playerPreviews.Length) % playerPreviews.Length;
        // Set the new selected player preview
        selectedPlayerPreview.SetActive(false);
        selectedPlayerPreview = playerPreviews[previousIndex];
        selectedPlayerPreview.SetActive(true);
    }

    public void ChangeScene(string sceneName)
    {
        if (sceneName.Equals("Quit", System.StringComparison.OrdinalIgnoreCase))
        {
            ExitGame();
        }
        else
        {
            // Lưu scene đích vào static variable trước khi chuyển sang LoadingScene
            Loading.TargetSceneName = sceneName;
            // Chuyển sang LoadingScene
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Dừng chơi trong Editor
#endif
    }
}