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
            selectedIndex = GameData.instance.selectedCharacterIndex;
        }

        // Tạo ra nhân vật tương ứng với lựa chọn
        if (playerPrefabs.Length > selectedIndex)
        {
            Instantiate(playerPrefabs[selectedIndex], spawnPoint.position, Quaternion.identity);
        }
    }
}