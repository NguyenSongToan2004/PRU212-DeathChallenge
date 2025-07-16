using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideLogoController : MonoBehaviour
{
    [Header("Logo Slide Settings")]
    [SerializeField] private Canvas logoSlide;
    [SerializeField] private float timeLoading = 1f;

    private void Start()
    {
        if (logoSlide != null)
        {
            logoSlide.gameObject.SetActive(true);
            StartCoroutine(LogoSlideCoroutine());
        }
        else
        {
            Debug.LogWarning("LogoSlide canvas is not assigned.");
        }
    }

    /// <summary>
    /// Handles the fade-in and fade-out effect for each logo in the slide.
    /// </summary>
    private IEnumerator LogoSlideCoroutine()
    {
        if (logoSlide == null)
            yield break;

        foreach (Transform logo in logoSlide.transform)
        {
            if (logo == null) continue;

            CanvasGroup canvasGroup = logo.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = logo.gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            logo.gameObject.SetActive(true);

            // Fade In
            float elapsed = 0f;
            while (elapsed < timeLoading)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsed / timeLoading);
                yield return null;
            }

            // Hold the logo
            yield return new WaitForSeconds(1f);

            // Fade Out
            elapsed = 0f;
            while (elapsed < timeLoading)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / timeLoading));
                yield return null;
            }

            logo.gameObject.SetActive(false);
        }

        logoSlide.gameObject.SetActive(false);

        ChangeScene();
    }

    private void ChangeScene()
    {
        StartCoroutine(FadeOnSceneChange("LoadingScene"));
    }

    private IEnumerator FadeOnSceneChange(string sceneName)
    {
        yield return new WaitForSeconds(timeLoading);
        SceneManager.LoadScene(sceneName);
    }
}