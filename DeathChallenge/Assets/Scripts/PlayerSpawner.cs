using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Kéo các Prefab nhân vật có thể chơi vào đây
    // Thứ tự phải khớp với thứ tự trong playerPreviews của MenuController
    public GameObject[] playerPrefabs;
    public Transform spawnPoint; // Vị trí tạo nhân vật

    void Start()
    {
        int selectedIndex = 0;
        if (GameData.instance != null)
        {
            Debug.Log("GameData instance đã được tìm thấy, sử dụng chỉ số đã lưu.");
            Debug.Log(GameData.instance.selectedCharacterIndex);

            selectedIndex = GameData.instance.selectedCharacterIndex;
            Debug.Log("check " +GameData.instance.selectedCharacterIndex);
        }
        else
        {
            Debug.LogWarning("GameData instance không tồn tại, sử dụng chỉ số mặc định 0.");
            selectedIndex = 0;
        }

        // Tạo ra nhân vật tương ứng với lựa chọn
        if (playerPrefabs.Length > selectedIndex)
        {
            playerPrefabs[selectedIndex].SetActive(true); // Kích hoạt prefab đã chọn
            //Instantiate(playerPrefabs[selectedIndex], spawnPoint.position, Quaternion.identity);
        }
    }
}