using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  
    public float increasedSpeed = 10f;  
    public float currentSpeed;
    public float jumpForce = 20f;
    public float fallMultiplier = 3.5f;
    public float jumpMultiplier = 4.5f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isInTimeZone = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        Move();
        
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }

        // 추가 중력을 적용해 점프 후 빠르게 내려오도록 조작감 개선용... 코드

         // 플레이어가 내려올 때,
        if (rb.velocity.y < 0)
        {
            // 추가 중력 적용
            rb.gravityScale = fallMultiplier; 
        }
        // 위로 점프했을 때
        else if (rb.velocity.y > 0) 
        {
            // 위로 점프 시 중력 배수 적용
            rb.gravityScale = jumpMultiplier; 
        }
        else
        {
            rb.gravityScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Move()
    {
        float moveX = 0;

        if (Input.GetKey(KeyCode.D))
        {
            //오른쪽 이동
            moveX = 1; 
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //왼쪽 이동
            moveX = -1; 
        }

        rb.velocity = new Vector2(moveX * currentSpeed, rb.velocity.y); 


        //방향 전환하기
        if (moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
        isGrounded = false;  
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // 총알을 firePoint 위치에서 생성
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // SATCollisionObject.cs가 제대로 추가되었는지 확인
            SATCollisionObject bulletSAT = bullet.GetComponent<SATCollisionObject>();
            if (bulletSAT == null)
            {
                Debug.LogError("생성된 Bullet에 SATCollisionObject가 없다.");
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

    // TimeZone에 들어갔을 때 속도 증가
    public void EnterTimeZone()
    {
        if (!isInTimeZone)
        {
            isInTimeZone = true;
            // TimeZone 안에서 속도 증가
            currentSpeed = increasedSpeed;  
        }
    }

    // TimeZone을 벗어났을 때 속도 복원
    public void ExitTimeZone()
    {
        if (isInTimeZone)
        {
            isInTimeZone = false;
             // 원래 속도로 복원
            currentSpeed = moveSpeed; 
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;

        // X축 스케일*(-1)로 전환
        localScale.x *= -1;  
        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            isGrounded = true; 
        }
    }
}
