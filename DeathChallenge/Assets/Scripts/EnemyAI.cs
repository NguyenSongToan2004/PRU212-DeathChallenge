using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour
{
    // === CÁC BIẾN CÓ THỂ TINH CHỈNH TRONG UNITY ===
    [Header("Player & Patrol")]
    public Transform player; // Kéo GameObject của người chơi vào đây
    public Transform[] patrolPoints; // Mảng chứa các điểm tuần tra (Point A, Point B)
    public float moveSpeed = 2f; // Tốc độ di chuyển

    [Header("AI Detection")]
    public float chaseRange = 5f; 
    public float attackRange = 1f;

    [Header("Attack Settings")]
    public int attackDamage = 100;
    public float knockbackForce = 100f;

    // === TRẠNG THÁI CỦA AI ===
    private enum AIState { Patrol, Chase, Attack }
    private AIState currentState;

    // === BIẾN NỘI BỘ ===
    private int currentPatrolIndex;
    private float distanceToPlayer;

    [Header("Patrol Settings")]
    public float patrolWaitTime = 2f; // Thời gian chờ tại mỗi điểm tuần tra
    private bool isWaiting = false; // Cờ để kiểm tra trạng thái chờ

    private Animator animator; // Nếu có Animator, có thể dùng để điều khiển hoạt ảnh

    private bool isPlayerDead = false;


    void Start()
    {
        // Gán trạng thái ban đầu là tuần tra
        currentState = AIState.Patrol;
        currentPatrolIndex = 0;
        animator = GetComponent<Animator>();

        // Tự động tìm người chơi nếu chưa được gán
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (isPlayerDead)
        {
            Patrol();
            return; // Thoát khỏi hàm Update ngay lập tức
        }

        // Tính khoảng cách tới người chơi mỗi frame
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Bộ não của AI - Quyết định hành động dựa trên trạng thái hiện tại
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                // Điều kiện chuyển trạng thái: Nếu người chơi trong tầm đuổi theo
                if (distanceToPlayer < chaseRange)
                {
                    currentState = AIState.Chase;
                }
                break;

            case AIState.Chase:
                Chase();
                // Điều kiện chuyển trạng thái: Nếu người chơi ngoài tầm -> quay lại tuần tra
                if (distanceToPlayer > chaseRange)
                {
                    animator.SetBool("isWalking", true); // Bật hoạt ảnh đi bộ
                    currentState = AIState.Patrol;
                }
                // Điều kiện chuyển trạng thái: Nếu người chơi trong tầm tấn công
                else if (distanceToPlayer < attackRange)
                {
                    animator.SetBool("isWalking", false); // Bật hoạt ảnh đi bộ
                    currentState = AIState.Attack;
                }
                break;

            case AIState.Attack:
                Attack();
                // Điều kiện chuyển trạng thái: Nếu người chơi thoát khỏi tầm tấn công
                if (distanceToPlayer > attackRange)
                {
                    currentState = AIState.Chase;
                    animator.SetBool("isAttacking", false); // Tắt hoạt ảnh tấn công
                }
                break;
        }
    }

    void Patrol()
    {
        // Nếu đang trong trạng thái chờ, không làm gì cả
        if (isWaiting) return;

        // Nếu không có điểm tuần tra, đứng yên
        if (patrolPoints.Length == 0) return;

        // Lấy điểm tuần tra mục tiêu
        Transform targetPoint = patrolPoints[currentPatrolIndex];

        // Di chuyển về phía điểm đó
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        // Nếu đã đến gần điểm đó, bắt đầu coroutine để chờ
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Bắt đầu thực thi hàm chờ
            StartCoroutine(WaitAtPoint());
        }
    }

    // THÊM HÀM MỚI NÀY VÀO SCRIPT CỦA BẠN
    IEnumerator WaitAtPoint()
    {
        // 1. Đặt trạng thái là đang chờ
        isWaiting = true;
        animator?.SetBool("isWalking", false); 

        // 2. Yêu cầu Unity chờ trong 'patrolWaitTime' giây
        yield return new WaitForSeconds(patrolWaitTime);

        // 3. Sau khi chờ xong, chuyển sang điểm tuần tra tiếp theo
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        // 4. Tắt trạng thái chờ để AI có thể di chuyển trở lại
        isWaiting = false;
        animator.SetBool("isWalking", true);
    }

    void Chase()
    {
        // Di chuyển về phía người chơi
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        animator.SetBool("isAttacking", true); 
        Debug.Log(gameObject.name + " is attacking the player!");
    }

    // --- VẼ CÁC VÙNG PHÁT HIỆN ĐỂ DỄ KIỂM TRA TRONG EDITOR ---
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TriggerAttack()
    {
        // Tìm đối tượng người chơi trong phạm vi tấn công
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D playerCollider in hitPlayers)
        {
            // Kiểm tra xem đối tượng va chạm có phải là người chơi không
            if (playerCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    // Gây sát thương
                    playerHealth.TakeDamage(attackDamage);

                    Vector2 knockbackDirection = (playerCollider.transform.position - transform.position).normalized;
                    playerHealth.ApplyKnockback(knockbackDirection, knockbackForce);
                }
                // Dùng break nếu bạn chỉ muốn đòn đánh trúng một mục tiêu
                break;
            }
        }
    }

    public void OnPlayerDied()
    {
        isPlayerDead = true; // BÁO HIỆU NGƯỜI CHƠI ĐÃ CHẾT
        currentState = AIState.Patrol; // Chuyển về trạng thái tuần tra

        // Tắt các hoạt ảnh không cần thiết
        animator?.SetBool("isAttacking", false);
        animator?.SetBool("isWalking", true); // Đảm bảo nó bắt đầu đi tuần tra
    }
}