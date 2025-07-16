using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] playerPrefabs; // Danh sách prefab thực tế người chơi

    void Start()
    {
        if (MenuController.selectedPlayerPreview == null)
        {
            Debug.LogWarning("Không có nhân vật được chọn từ scene trước.");
            return;
        }

        GameObject selected = MenuController.selectedPlayerPreview;

        // Lấy script Image từ preview đã chọn
        KeyPlayer imageComponent = selected.GetComponent<KeyPlayer>();
        if (imageComponent == null)
        {
            Debug.LogError("Không tìm thấy script KeyPlayer trên selectedPlayerPreview.");
            return;
        }

        string selectedKey = imageComponent.playerKey;
        Debug.Log("Đã chọn nhân vật với key: " + selectedKey);

        // Tìm prefab có key trùng khớp
        GameObject prefabToSpawn = null;
        foreach (GameObject prefab in playerPrefabs)
        {
            KeyPlayer prefabImage = prefab.GetComponent<KeyPlayer>();
            if (prefabImage != null && prefabImage.playerKey == selectedKey)
            {
                prefabToSpawn = prefab;
                break;
            }
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("Không tìm thấy prefab nào trùng key: " + selectedKey);
            return;
        }

        // Spawn nhân vật tại vị trí mong muốn
        Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
    }
}
