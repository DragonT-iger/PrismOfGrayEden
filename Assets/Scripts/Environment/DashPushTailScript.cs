using UnityEngine;

public class DashPushTailScript : MonoBehaviour
{
    [SerializeField] private float _forceMagnitude = 10f; // 가할 힘의 크기
    public bool IsThisPushingNow;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 객체에 Rigidbody2D가 있는지 확인

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb == null || IsThisPushingNow == false || collision.gameObject.CompareTag("Player"))
        {
            return;
        }
            // 충돌 지점 정보 가져오기
            ContactPoint2D contact = collision.GetContact(0);

            // 충돌 지점의 법선 벡터 (충돌 면의 수직 방향)
            Vector2 collisionNormal = contact.normal;

            // 충돌 방향 반대로 (자신에서 상대 방향으로)
            Vector2 forceDirection = -collisionNormal;

            // 가장 가까운 상하좌우 방향으로 클램핑
            Vector2 clampedDirection = ClampToCardinalDirection(forceDirection);
        // 현재 Pause된 상태라면
        if (collision.gameObject.GetComponent<IPausable>().IsPaused == true)
        {
            // 저장된 힘만 바꾸기
            collision.gameObject.GetComponent<IPausable>().SetSavedVelocity(clampedDirection * _forceMagnitude);
            return;
        }
            // 힘 적용
            rb.linearVelocity = clampedDirection * _forceMagnitude;
        Debug.Log($"{rb.gameObject.name}에 힘 적용, 현재 힘 {rb.linearVelocity}");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌한 객체에 Rigidbody2D가 있는지 확인

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb == null || IsThisPushingNow == false || collision.gameObject.CompareTag("Player"))
        {
            return;
        }
            // 충돌 지점 정보 가져오기
            ContactPoint2D contact = collision.GetContact(0);

            // 충돌 지점의 법선 벡터 (충돌 면의 수직 방향)
            Vector2 collisionNormal = contact.normal;

            // 충돌 방향 반대로 (자신에서 상대 방향으로)
            Vector2 forceDirection = -collisionNormal;

            // 가장 가까운 상하좌우 방향으로 클램핑
            Vector2 clampedDirection = ClampToCardinalDirection(forceDirection);
        // 현재 Pause된 상태라면
        if (collision.gameObject.GetComponent<IPausable>().IsPaused == true)
        {
            // 저장된 힘만 바꾸기
            collision.gameObject.GetComponent<IPausable>().SetSavedVelocity(clampedDirection * _forceMagnitude);
            return;
        }
            // 힘 적용
            rb.linearVelocity = clampedDirection * _forceMagnitude;
        Debug.Log($"{rb.gameObject.name}에 힘 적용, 현재 힘 {rb.linearVelocity}");
    }

    // 벡터를 상하좌우 중 가장 가까운 방향으로 클램핑하는 함수
    private Vector2 ClampToCardinalDirection(Vector2 direction)
    {
        // 방향 벡터 정규화
        direction.Normalize();

        // x와 y 성분의 절대값 비교
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // x 성분이 더 크면 좌우 방향
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            // y 성분이 더 크면 상하 방향
            return direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }
}