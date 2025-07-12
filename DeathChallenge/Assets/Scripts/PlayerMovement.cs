using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _dashTime;
    bool _isDashing = false;

    public GameObject ghostEffect;
    public float ghostDelaySeconds;

    private Coroutine dashEffectCoroutine;

    public float moveSpeed = 5f; // Tốc độ di chuyển của nhân vật
    public float dashBoost;
    public float dashTime;
    // --- PHẦN THÊM MỚI ---
    public float yLimit = 2.5f; // Giới hạn chiều cao Y mà nhân vật có thể đạt tới
    // ----------------------

    private Rigidbody2D rigidbody2D;

    private Vector2 movementInput;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PlayerHealth.instance && PlayerHealth.instance.isDead)
        {
            Debug.Log("Player is dead, stopping movement.");
            return;
        }

        if (Keyboard.current.spaceKey.isPressed && _dashTime <= 0 && !_isDashing)
        {
            moveSpeed += dashBoost;
            _dashTime = dashTime;
            _isDashing = true;
            StartDashEffect();
        }

        if (_dashTime <= 0 && _isDashing)
        {
            moveSpeed -= dashBoost;
            _isDashing = false;
            StopDashEffect();
        }
        else
        {
            _dashTime -= Time.deltaTime;
        }

        // Lấy input từ hệ thống Input System mới
        movementInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) movementInput.y += 1;
            if (Keyboard.current.sKey.isPressed) movementInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) movementInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) movementInput.x += 1;
        }

        // --- PHẦN THÊM MỚI ---
        // Kiểm tra nếu nhân vật đang ở hoặc vượt quá giới hạn Y và đang cố đi lên
        if (transform.position.y >= yLimit && movementInput.y > 0)
        {
            movementInput.y = 0; // Không cho phép di chuyển lên nữa
        }

        // Chuẩn hóa vector để không di chuyển nhanh hơn khi đi chéo
        if (movementInput.sqrMagnitude > 1)
            movementInput = movementInput.normalized;

        // Di chuyển nhân vật
        transform.position += new Vector3(movementInput.x, movementInput.y, 0f) * moveSpeed * Time.deltaTime;

        animator.SetFloat("Speed", movementInput.sqrMagnitude);

    }

    void StartDashEffect()
    {
        if (dashEffectCoroutine != null)
        {
            StopCoroutine(dashEffectCoroutine);
        }

        dashEffectCoroutine = StartCoroutine(DashEffectCoroutine());
    }

    void StopDashEffect()
    {
        if (dashEffectCoroutine != null)
        {
            StopCoroutine(dashEffectCoroutine);
        }

    }

    IEnumerator DashEffectCoroutine()
    {
        while (true)
        {
            GameObject ghost = Instantiate(ghostEffect, transform.position, transform.rotation);
            Sprite currenSprite = GetComponent<SpriteRenderer>().sprite;
            ghost.GetComponent<SpriteRenderer>().sprite = currenSprite;

            Destroy(ghost, 0.5f); 
            yield return new WaitForSeconds(ghostDelaySeconds);
        }
    }
}
