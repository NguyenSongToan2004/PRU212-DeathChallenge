using UnityEngine;
using System.Collections; // Bắt buộc phải có để dùng Coroutine
using TMPro; // Bắt buộc phải có để dùng TextMeshPro

public class IntroController : MonoBehaviour
{
    public TextMeshProUGUI introText; // Kéo Text của bạn vào đây
    public float displayTime = 5f; // Thời gian hiển thị (5 giây)

    // Start is called before the first frame update
    void Start()
    {
        // Bắt đầu Coroutine để xử lý việc hiển thị và ẩn đi
        StartCoroutine(ShowAndHideIntro());
    }

    private IEnumerator ShowAndHideIntro()
    {
        // 1. Hiển thị text lúc đầu
        introText.gameObject.SetActive(true);

        // 2. Chờ trong 'displayTime' giây
        yield return new WaitForSeconds(displayTime);

        // 3. Sau khi chờ xong, ẩn text đi
        introText.gameObject.SetActive(false);
    }
}