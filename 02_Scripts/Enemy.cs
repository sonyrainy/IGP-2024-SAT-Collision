using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;  // 적의 체력

    // 적에게 데미지를 입히는 함수
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage! Remaining health: {health}");

        // 체력이 0 이하일 때 적 제거
        if (health <= 0)
        {
            Die();
        }
    }

    // 적이 죽는 함수
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);  // 적 오브젝트 삭제
    }
}
