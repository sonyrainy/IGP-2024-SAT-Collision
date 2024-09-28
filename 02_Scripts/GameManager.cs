using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject timeZonePrefab;        // TimeZone 프리팹
    public float timeZoneLifetime = 5f;      // TimeZone이 유지되는 시간

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // 이미 존재하는 인스턴스가 있으면 파괴
        }
        else
        {
            Instance = this;  // 현재 인스턴스를 설정
            DontDestroyOnLoad(gameObject);  // 씬 전환 시에도 파괴되지 않도록 설정
        }
    }

    void Update()
    {
        // 마우스 좌클릭으로 TimeZone 생성
        if (Input.GetMouseButtonDown(0))
        {
            CreateTimeZone();
        }
    }

    void FixedUpdate()
    {
        // 충돌 처리를 CollisionManager로 위임
        CollisionManager.Instance.HandleCollisions();
    }

    // 마우스 클릭 시 TimeZone 생성
    private void CreateTimeZone()
    {
        // 마우스 클릭한 위치를 가져오기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;  // 2D이므로 z값을 0으로 설정

        // TimeZone 오브젝트 생성
        GameObject timeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);

        // SATCollisionObject를 CollisionManager에 등록
        SATCollisionObject timeZoneSAT = timeZone.GetComponent<SATCollisionObject>();
        if (timeZoneSAT == null)
        {
            timeZoneSAT = timeZone.AddComponent<SATCollisionObject>();  // SATCollisionObject가 없으면 추가
        }

        CollisionManager.Instance.RegisterTimeZone(timeZoneSAT);

        // 일정 시간이 지나면 TimeZone을 삭제
        Destroy(timeZone, timeZoneLifetime);
    }
}
