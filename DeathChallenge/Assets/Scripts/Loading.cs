using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static Loading Instance { get; private set; }
    public static string TargetSceneName { get; set; } = ""; // Static để lưu trữ giữa các scene

    [Header("UI References")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingScreen; // Image.type must be Filled
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Loading Settings")]
    [SerializeField] private bool showSlider = true;
    [SerializeField] private float timeLoading = 1f;
    [SerializeField] private float fadeDuration = 1f; // Thêm setting cho fade duration
    private const float maxFakeProgress = 0.9f;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        // Đảm bảo loading panel được kích hoạt từ đầu
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // Nếu có target scene thì load nó, ngược lại load MainMenu
        string sceneToLoad = !string.IsNullOrEmpty(TargetSceneName) ? TargetSceneName : "MainMenu";

        // Chạy fade-in trước, sau đó mới load scene
        StartCoroutine(StartLoadingSequence(sceneToLoad));
    }
    private IEnumerator StartLoadingSequence(string sceneToLoad)
    {
        // Bước 1: Fade in loading panel
        yield return StartCoroutine(FadeInLoadingPanel());

        // Bước 2: Load scene sau khi fade in hoàn tất
        yield return StartCoroutine(LoadSceneCoroutine(sceneToLoad));
    }

    private IEnumerator FadeInLoadingPanel()
    {
        if (loadingPanel != null)
        {
            CanvasGroup canvasGroup = loadingPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = loadingPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            // Fade in effect
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                Debug.Log($"Fading in: {elapsed}/{fadeDuration}");
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null; // Quan trọng: yield return null thay vì WaitForSeconds
            }

            // Đảm bảo alpha = 1 khi hoàn thành
            canvasGroup.alpha = 1f;
        }
        else
        {
            Debug.LogWarning("Loading panel is not assigned.");
        }
    }

    /// <summary>
    /// Đặt scene đích sẽ được load sau khi hiển thị loading screen
    /// </summary>
    public void SetTargetScene(string sceneName)
    {
        TargetSceneName = sceneName;
    }

    /// <summary>
    /// Gọi load scene kèm hiệu ứng loading.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        // Nếu sceneName là "LoadingScene" thì chỉ cần chuyển scene
        if (sceneName == "LoadingScene")
        {
            SceneManager.LoadScene("LoadingScene");
            return;
        }

        // Ngược lại, set target scene và chuyển sang LoadingScene
        SetTargetScene(sceneName);
        SceneManager.LoadScene("LoadingScene");
    }

    private void ValidateReferences()
    {
        if (loadingPanel == null)
        {
            loadingPanel = GameObject.Find("LoadingPanel");
            if (loadingPanel == null)
            {
                Debug.LogWarning("LoadingPanel not found! This is normal if not in LoadingScene.");
                return;
            }
        }

        if (loadingScreen == null)
        {
            loadingScreen = loadingPanel.GetComponentInChildren<Image>();
        }

        if (loadingText == null)
        {
            loadingText = loadingPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// Load scene với fade effect
    /// </summary>
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        if (loadingPanel != null && !loadingPanel.activeInHierarchy)
        {
            yield return StartCoroutine(FadeInLoadingPanel());
        }
        yield return StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /// <summary>
    /// Hiệu ứng loading bar trong khi load scene.
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (!loadOperation.isDone)
        {
            // Tính toán fake progress
            if (fakeProgress < maxFakeProgress)
            {
                fakeProgress += Time.deltaTime / timeLoading;
            }
            else if (loadOperation.progress >= 0.9f)
            {
                fakeProgress += Time.deltaTime / (timeLoading * 0.5f);
            }

            fakeProgress = Mathf.Clamp01(fakeProgress);

            // Cập nhật UI
            if (showSlider && loadingScreen != null)
                loadingScreen.fillAmount = fakeProgress;

            if (loadingText != null)
                loadingText.text = $"Loading... {Mathf.RoundToInt(fakeProgress * 100)}%";

            // Kích hoạt scene khi progress đạt 100%
            if (fakeProgress >= 1f)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Fade out loading panel (có thể dùng khi cần ẩn loading)
    /// </summary>
    public IEnumerator FadeOutLoadingPanel()
    {
        if (loadingPanel != null)
        {
            CanvasGroup canvasGroup = loadingPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                float elapsed = 0f;
                float startAlpha = canvasGroup.alpha;

                while (elapsed < fadeDuration)
                {
                    elapsed += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
                    yield return null;
                }

                canvasGroup.alpha = 0f;
                loadingPanel.SetActive(false);
            }
        }
    }
}