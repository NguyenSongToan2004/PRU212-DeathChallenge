using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


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

    [Header("Health Settings")]
    public int maxHealth = 300;

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

    [Header("Type monster")]
    public bool isRoaming = false;

    [Header("Roaming Settings (for ranged)")]
    public float roamRadius = 4f;
    public float roamTimer = 3f;
    public float safeDistance = 8f;
    public GameObject bulletPrefab; // Kéo prefab viên đạn vào đây
    public Transform bulletPoint; // Kéo đối tượng FirePoint vào đây

    [Header("UI Settings")]
    public GameObject damageTextPrefab;
    public Canvas gameCanvas;

    private bool isWaiting = false;
    private Animator animator;
    private bool isPlayerDead = false;
    private float currentRoamTime;
    private Vector2 roamPosition;
    public float maxYPosition = 9f; // Giới hạn Y để tránh quái vật bay lên quá cao\
    public float minYPosition = -10f; // Giới hạn Y để tránh quái vật bay xuống quá thấp

    private int currentHealth;
    private bool isEnemyDead = false;


    void Start()
    {
        // Gán trạng thái ban đầu là tuần tra
        currentState = AIState.Patrol;
        currentPatrolIndex = 0;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Tự động tìm người chơi nếu chưa được gán
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {

        if (isEnemyDead) return;

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
                    if (isRoaming)
                    {
                        PickNewRoamPosition();
                    }
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

    void LateUpdate()
    {
        // Lấy vị trí hiện tại của quái vật
        Vector3 currentPosition = transform.position;

        // "Kẹp" giá trị Y trong khoảng từ minYPosition đến maxYPosition
        float clampedY = Mathf.Clamp(currentPosition.y, minYPosition, maxYPosition);

        // Cập nhật lại vị trí với giá trị Y đã được giới hạn
        transform.position = new Vector3(currentPosition.x, clampedY, currentPosition.z);
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

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        animator?.SetBool("isWalking", false);

        yield return new WaitForSeconds(patrolWaitTime);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        isWaiting = false;
        animator.SetBool("isWalking", true);
    }

    //void Chase()
    //{
    //    // Di chuyển về phía người chơi
    //    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    //}

    void Chase()
    {
        // Logic cho quái đi lang thang (từ xa)
        if (isRoaming)
        {
            Debug.Log("Enemy is roaming");
            if (distanceToPlayer < safeDistance)
            {
                // Tìm hướng bay ra xa khỏi người chơi
                Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;

                // Di chuyển theo hướng đó
                transform.position += (Vector3)directionAwayFromPlayer * moveSpeed * Time.deltaTime;

                // Dừng các logic đuổi theo khác tại đây
                return;
            }

            currentRoamTime -= Time.deltaTime;
            if (currentRoamTime <= 0)
            {
                PickNewRoamPosition();
            }

            // Di chuyển tới vị trí lang thang, không phải người chơi
            transform.position = Vector2.MoveTowards(transform.position, roamPosition, moveSpeed * Time.deltaTime);
        }
        // Logic cho quái cận chiến (như cũ)
        else
        {
            // Di chuyển thẳng về phía người chơi
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void PickNewRoamPosition()
    {
        // Đặt lại đồng hồ đếm ngược
        currentRoamTime = roamTimer;

        // Tìm một điểm ngẫu nhiên trong vòng tròn quanh người chơi
        roamPosition = (Vector2)player.position + Random.insideUnitCircle * roamRadius;
    }

    public void TakeDamage(int damage, bool isCritical)
    {
        // Nếu đã chết thì không nhận thêm sát thương
        if (isEnemyDead) return;

        currentHealth -= damage;

        if (damageTextPrefab != null && gameCanvas != null)
        {
            DamagePopup.Create(transform.position, damage, isCritical, damageTextPrefab, gameCanvas);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHurt");
        }
    }

    private void Die()
    {
        // Đánh dấu là đã chết để không chạy các logic khác
        isEnemyDead = true;

        Debug.Log(gameObject.name + " has died.");

        // Kích hoạt hoạt ảnh chết
        animator.SetTrigger("isDead");

        // Vô hiệu hóa các thành phần để quái không còn tương tác vật lý
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Bắt đầu đếm ngược 3 giây trước khi biến mất
        StartCoroutine(DeactivateAfterDelay(3f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        // Chờ trong 'delay' giây
        yield return new WaitForSeconds(delay);

        // Vô hiệu hóa toàn bộ GameObject
        gameObject.SetActive(false);
    }

    void Attack()
    {
        if (isRoaming && distanceToPlayer < safeDistance)
        {
            animator.SetBool("isAttacking", false);
            currentState = AIState.Chase;
            return; // Dừng hàm Attack() tại đây
        }
        animator.SetBool("isAttacking", true);
    }

    // --- VẼ CÁC VÙNG PHÁT HIỆN ĐỂ DỄ KIỂM TRA TRONG EDITOR ---
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Vẽ vùng an toàn (VÙNG MỚI)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, safeDistance);
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

    public void FireProjectile()
    {
        if (bulletPrefab == null || bulletPoint == null || player == null || isPlayerDead)
        {
            return;
        }

        // Tạo ra viên đạn tại vị trí và góc quay của FirePoint
        GameObject projectileObj = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
        Projectile projectileScript = projectileObj.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            // Tính toán hướng bắn về phía người chơi
            Vector3 direction = (player.transform.position - bulletPoint.transform.position).normalized;
            // Gọi hàm Fire để viên đạn bay
            projectileScript.Fire(direction);
        }
    }
}