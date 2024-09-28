using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;  

    // 적이 데미지를 입는 함수
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Enemy가 {damage}의 데미지를 받았고, 현재 적의 체력은 {health}이다.");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("적의 HP가 0 이하가 되었다.");
        Destroy(gameObject); 
    }
}
