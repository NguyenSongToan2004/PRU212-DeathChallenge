using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public int attackDamage; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hitbox touched the Player!");
            EnemyAI enemyHealth = other.GetComponent<EnemyAI>();
            PlayerAttack playerAttack = GetComponentInParent<PlayerAttack>();
            if (enemyHealth != null)
            {
                // --- LOGIC TÍNH CHÍ MẠNG ---
                bool isCritical = false;
                int finalDamage = playerAttack.baseDamage; // Lấy sát thương cơ bản từ PlayerAttack hoặc script khác

                // Tung số ngẫu nhiên từ 1 đến 100
                float randomValue = Random.Range(1, 101);
                if (randomValue <= playerAttack.critChance) // Ví dụ: critChance = 20, nếu random ra số <= 20 thì là chí mạng
                {
                    isCritical = true;
                    finalDamage = Mathf.RoundToInt(playerAttack.baseDamage * playerAttack.critMultiplier); // Nhân sát thương lên
                }

                // Gọi hàm TakeDamage với sát thương và trạng thái chí mạng cuối cùng
                enemyHealth.TakeDamage(finalDamage, isCritical);
                // -----------------------------
            }

            gameObject.SetActive(false);
        }
    }
}