using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // Biến starPos nên đặt tên là startPos cho đúng chính tả
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        // Kiểm tra xem có SpriteRenderer không để tránh lỗi NullReferenceException

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            length = sr.bounds.size.x;
        }
        else
        {
            Debug.LogError("Không tìm thấy SpriteRenderer trên GameObject " + gameObject.name);
            length = 0;
        }
        startPos = transform.position.x;
    }

    void FixedUpdate()
    {
        if (cam == null)
        {
            Debug.LogError("Chưa gán Camera cho BackgroundController trên " + gameObject.name);
            return;
        }

        float distance = (cam.transform.position.x * parallaxEffect);
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
