using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI durationValue;
    [SerializeField] private GameObject enterNamePanel;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject highscoreUIElementPrefab;
    [SerializeField] private RectTransform elementContainer;
    [SerializeField] private TMP_InputField nameInputField;

    private HighscoreHandler highscoreHandler;
    private int durationInSeconds;
    private readonly List<GameObject> uiElements = new();
    // Tạm hard‑code danh sách map vừa chơi
    private readonly List<string> maps = new() { "Map_1", "Map_2", "Map_3" };

    private void Awake()
    {
        HighscoreHandler.OnHighscoreListChanged += UpdateUI;
    }

    private void Start()
    {
        // 1) Tính và hiển thị duration
        UpdateDuration();

        // 2) Lấy handler và kiểm tra
        highscoreHandler = FindObjectOfType<HighscoreHandler>();
        if (highscoreHandler == null)
        {
            Debug.LogError("HighscoreHandler not found in scene!");
            return;
        }

        // 3) Show panel, kiểm tra index, bật panel nhập tên
        ShowPanel();
        int idx = highscoreHandler.GetInsertIndex(durationInSeconds);
        enterNamePanel.SetActive(idx >= 0 && idx < highscoreHandler.MaxCount);

        // 4) Đợi frame đầu rồi fill UI
        StartCoroutine(DelayedUpdateUI());
    }

    private void UpdateDuration()
    {
        if (durationValue == null) return;
        // Giả sử lấy giờ hiện tại trừ đi startAt cố định
        string hhmmss = DateTime.Now.ToString("HH:mm:ss");
        durationValue.text = hhmmss;
        // Chuyển sang giây (chỉ ví dụ)
        TimeSpan ts = TimeSpan.Parse(hhmmss);
        durationInSeconds = (int)ts.TotalSeconds;
    }

    private IEnumerator DelayedUpdateUI()
    {
        yield return null;
        UpdateUI(highscoreHandler.CurrentHighscores);
    }

    public void OnSaveButtonClick()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName)) return;

        var entry = new HighscoreElement(playerName, durationInSeconds);
        highscoreHandler.AddHighscoreAt(durationInSeconds, entry);
        enterNamePanel.SetActive(false);
    }

    private void UpdateUI(List<HighscoreElement> list)
    {
        // 1) Tạo hoặc tái sử dụng prefab để match số bản ghi
        for (int i = 0; i < list.Count; i++)
        {
            if (i >= uiElements.Count)
            {
                var go = Instantiate(highscoreUIElementPrefab, elementContainer);
                uiElements.Add(go);
            }

            var inst = uiElements[i];
            inst.SetActive(true);

            var texts = inst.GetComponentsInChildren<TextMeshProUGUI>();
            // Giả sử prefab có 4 TMP Text: [0]=Rank, [1]=Name, [2]=Maps, [3]=Duration
            texts[0].text = (i + 1).ToString();
            texts[1].text = list[i].playerName;
            texts[2].text = maps.Count > 0
                ? string.Join("\n", maps)
                : "No maps";
            texts[3].text = TimeSpan.FromSeconds(list[i].duration).ToString(@"hh\:mm\:ss");
        }

        // 2) Ẩn thừa
        for (int j = list.Count; j < uiElements.Count; j++)
            uiElements[j].SetActive(false);
    }

    private void OnDestroy()
    {
        HighscoreHandler.OnHighscoreListChanged -= UpdateUI;
    }

    public void ShowPanel() => panel.SetActive(true);
    public void ClosePanel() => panel.SetActive(false);
}
