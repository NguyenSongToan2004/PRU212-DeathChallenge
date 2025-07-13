using UnityEngine;
using UnityEngine.InputSystem; // Hoặc Input Manager cũ

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public HitboxController attackHitbox; // Kéo Hitbox của Player vào đây
    public float attackCooldown = 2f; // Thời gian hồi chiêu
    private float lastAttackTime;
    public float critChance = 20f; // Tỷ lệ chí mạng: 20%
    public float critMultiplier = 2f; // Sát thương chí mạng sẽ nhân 2
    public int baseDamage = 50; // Sát thương cơ bản của đòn đánh

    void Start()
    {
        animator = GetComponent<Animator>();
        // Gán sát thương cho hitbox nếu cần
        attackHitbox.attackDamage = baseDamage; 
    }

    void Update()
    {
        // Kiểm tra nếu người chơi nhấn nút tấn công (ví dụ: chuột trái)
        // và đã hết thời gian hồi chiêu

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối
        animator.SetTrigger("isAttack");
    }

    public void EnableAttack()
    {
        if (attackHitbox != null)
        {
            attackHitbox.gameObject.SetActive(true);
        }
    }

    public void DisableAttack()
    {
        if (attackHitbox != null)
        {
            attackHitbox.gameObject.SetActive(false);
        }
    }
}