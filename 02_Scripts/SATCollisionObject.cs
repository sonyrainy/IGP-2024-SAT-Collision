using System.Linq;
using UnityEngine;

public class SATCollisionObject : MonoBehaviour
{
    private PolygonCollider2D polyCollider;

    void Start()
    {
        // PolygonCollider2D 컴포넌트 가져오기
        polyCollider = GetComponent<PolygonCollider2D>();

        if (polyCollider == null)
        {
            //Debug.LogError($"{gameObject.name}에 PolygonCollider2D가 없습니다.");
        }
    }

    // 꼭짓점 배열을 반환하는 함수
    public Vector2[] GetVertices()
    {
        if (polyCollider != null)
        {
            // 꼭짓점을 월드 좌표계로 변환하여 반환
            return polyCollider.points.Select(point => (Vector2)transform.TransformPoint(point)).ToArray();
        }
        else
        {
            return null;
        }
}

}
