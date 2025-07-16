using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static GameObject selectedPlayerPreview; // Reference to the selected player preview
    // List player preview list
    [SerializeField] private GameObject[] playerPreviews;
    private int currentIndex = 0;


    //public void Start()
    //{
    //    // Tắt tất cả các preview trước
    //    foreach (var preview in playerPreviews)
    //    {
    //        preview.SetActive(false);
    //    }

    //    // Hiển thị preview đầu tiên
    //    if (playerPreviews.Length > 0)
    //    {
    //        currentIndex = 0;
    //        playerPreviews[currentIndex].SetActive(true);
    //        UpdateSelectedCharacter();
    //    }
    //}

    public void Start()
    {
        // Set the first player preview as selected by default
        if (playerPreviews.Length > 0)
        {
            selectedPlayerPreview = playerPreviews[0];
            selectedPlayerPreview.SetActive(true);
        }

        // 🔍 Kiểm tra và log
        if (selectedPlayerPreview != null)
        {
            Debug.Log("Nhân vật đang được chọn: " + selectedPlayerPreview.name);
        }
        else
        {
            Debug.LogWarning("Chưa có nhân vật nào được chọn.");
        }
    }

    private void UpdateSelectedCharacter()
    {
        // Lưu chỉ số của nhân vật được chọn vào GameData
        if (GameData.instance != null)
        {
            GameData.instance.selectedCharacterIndex = currentIndex;
        }
    }

    //public void NextPlayer()
    //{
    //    // Find the index of the currently active player preview
    //    int currentIndex = System.Array.IndexOf(playerPreviews, selectedPlayerPreview);
    //    // Calculate the next index, wrapping around if necessary
    //    int nextIndex = (currentIndex + 1) % playerPreviews.Length;
    //    // Set the new selected player preview
    //    selectedPlayerPreview.SetActive(false);
    //    selectedPlayerPreview = playerPreviews[nextIndex];
    //    selectedPlayerPreview.SetActive(true);
    //}

    public void NextPlayer()
    {
        playerPreviews[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % playerPreviews.Length;
        playerPreviews[currentIndex].SetActive(true);
        UpdateSelectedCharacter();
    }

    //public void PreviousPlayer()
    //{
    //    // Find the index of the currently active player preview
    //    int currentIndex = System.Array.IndexOf(playerPreviews, selectedPlayerPreview);
    //    // Calculate the previous index, wrapping around if necessary
    //    int previousIndex = (currentIndex - 1 + playerPreviews.Length) % playerPreviews.Length;
    //    // Set the new selected player preview
    //    selectedPlayerPreview.SetActive(false);
    //    selectedPlayerPreview = playerPreviews[previousIndex];
    //    selectedPlayerPreview.SetActive(true);
    //}

    public void PreviousPlayer()
    {
        playerPreviews[currentIndex].SetActive(false);
        currentIndex = (currentIndex - 1 + playerPreviews.Length) % playerPreviews.Length;
        playerPreviews[currentIndex].SetActive(true);
        UpdateSelectedCharacter();
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