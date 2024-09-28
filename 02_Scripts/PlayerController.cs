using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 기본 이동 속도
    public float increasedSpeed = 10f;  // TimeZone 안에서 증가된 속도
    public float currentSpeed;  // 현재 적용된 속도
    public float jumpForce = 20f;
    public float fallMultiplier = 3.5f;
    public float jumpMultiplier = 4.5f;

    public GameObject bulletPrefab;  // 총알 프리팹
    public Transform firePoint;  // 총알이 발사되는 위치
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isInTimeZone = false;  // TimeZone 안에 있는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;  // 처음에는 기본 속도로 설정
    }

    void Update()
    {
        // 이동 처리
        Move();
        
        // 점프 처리
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }

        // 추가 중력 적용: 점프 후 빠르게 내려오는 처리
        if (rb.velocity.y < 0) // 플레이어가 내려오는 중일 때
        {
            rb.gravityScale = fallMultiplier; // 추가 중력 적용
        }
        else if (rb.velocity.y > 0) // 위로 점프했을 때
        {
            rb.gravityScale = jumpMultiplier; // 위로 점프 시 중력 배수 적용
        }
        else
        {
            rb.gravityScale = 1f; // 기본 중력
        }

        // Spacebar로 총알 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    // 플레이어 이동 로직 (A, D)
    void Move()
    {
        float moveX = 0;

        // A, D 입력 감지
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;  // 오른쪽으로 이동
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;  // 왼쪽으로 이동
        }

        // Rigidbody를 이용한 이동 처리
        rb.velocity = new Vector2(moveX * currentSpeed, rb.velocity.y);  // X축 이동, Y축은 기존 값 유지

        // 방향 전환 처리
        if (moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // 점프 로직
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Y축 방향으로 고정된 힘으로 점프
        isGrounded = false;  // 점프 중에는 바닥에서 떨어짐
    }

    // 총알 발사 로직
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // 총알을 firePoint 위치에서 생성
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // SATCollisionObject가 제대로 추가되었는지 확인
            SATCollisionObject bulletSAT = bullet.GetComponent<SATCollisionObject>();
            if (bulletSAT == null)
            {
                Debug.LogError("생성된 Bullet에 SATCollisionObject가 없습니다.");
            }

            // 플레이어가 바라보는 방향에 따라 총알 방향 설정
            float direction = isFacingRight ? 1f : -1f;

            // 총알의 Bullet 스크립트에 방향 전달
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);  // 총알의 방향 설정
            }

            // 총알을 SATManager에 등록
            CollisionManager.Instance.RegisterBullet(bulletSAT);
        }
        else
        {
            Debug.LogError("총알 프리팹 또는 firePoint가 설정되지 않았습니다.");
        }
    }

    // TimeZone에 들어갔을 때 속도 증가 (인터페이스 메서드 구현)
    public void EnterTimeZone()
    {
        if (!isInTimeZone)
        {
            isInTimeZone = true;
            currentSpeed = increasedSpeed;  // TimeZone 안에서 속도 증가
        }
    }

    // TimeZone을 벗어났을 때 속도 복원 (인터페이스 메서드 구현)
    public void ExitTimeZone()
    {
        if (isInTimeZone)
        {
            isInTimeZone = false;
            currentSpeed = moveSpeed;  // 원래 속도로 복원
        }
    }

    // 방향 전환 로직 (좌우 반전)
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;  // X축 스케일을 반전하여 방향을 바꿈
        transform.localScale = localScale;
    }

    // 바닥에 닿았는지 확인
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            isGrounded = true;  // 바닥에 닿았을 때 점프 가능 상태로 변경
        }
    }
}
