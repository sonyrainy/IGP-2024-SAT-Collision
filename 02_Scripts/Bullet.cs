using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;     // 기본 총알 속도
    public float increasedSpeed = 40f;  // TimeZone 안에 있을 때 증가한 총알 속도
    public float currentSpeed;          // 현재 적용된 속도
    public float originalSpeed;         // TimeZone에 들어가기 전 원래 속도

    public float damageMultiplier = 0.1f;  // 속도에 따른 데미지 배율
    private Rigidbody2D rb;
    private bool isInTimeZone = false;  // TimeZone 안에 있는지 여부

    // 추가: 총알 발사 방향을 받는 변수
    private float bulletDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = bulletSpeed;   // 기본 속도로 설정
        originalSpeed = bulletSpeed;  // TimeZone에 들어가기 전 속도를 저장

        // 초기 이동 시작
        MoveBullet(currentSpeed);
    }

    void MoveBullet(float speed)
    {
        rb.velocity = new Vector2(bulletDirection * speed, rb.velocity.y);  // 설정된 방향으로 이동
    }

    // TimeZone에 들어가면 속도 증가
    public void EnterTimeZone()
    {
        if (!isInTimeZone)
        {
            isInTimeZone = true;
            originalSpeed = currentSpeed;  // TimeZone에 들어가기 전 속도 저장
            currentSpeed = increasedSpeed; // TimeZone 안에서 속도 증가
            MoveBullet(currentSpeed);      // 변경된 속도 적용
        }
    }

    // TimeZone을 벗어나면 속도 복원
    public void ExitTimeZone()
    {
        if (isInTimeZone)
        {
            isInTimeZone = false;
            currentSpeed = originalSpeed;  // 속도를 TimeZone에 들어가기 전 상태로 복원
            MoveBullet(currentSpeed);      // 복원된 속도 적용
        }
    }

    // 플레이어가 바라보는 방향을 설정하는 함수
    public void SetDirection(float direction)
    {
        bulletDirection = direction;  // 방향 설정
    }

    // 총알의 속도에 따라 데미지를 계산하는 함수
    public float CalculateDamage()
    {
        float speed = rb.velocity.magnitude;  // 속도의 크기 계산
        return speed * damageMultiplier;      // 속도에 따른 데미지 배율 적용
    }
}
