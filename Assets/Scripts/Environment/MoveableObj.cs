using UnityEngine;

public class MoveableObj : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && false) // false를 shift 키 입력 받는 순간 바로 chain을 삭제 
        {
            Transform player = collision.gameObject.transform;
            transform.SetParent(player); // 부모로 설정

            Vector2 normal = collision.contacts[0].normal;
            Vector3 playerPos = player.position;
            Vector3 myPos = transform.position;

            Vector3 newPos = myPos;

            // 부딪힌 방향에 따라 해당 축의 위치를 조절
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                // 좌우 충돌일 경우 → x 위치 보정
                float xDiff = myPos.x - playerPos.x;
                float halfWidth = (GetComponent<Collider2D>().bounds.size.x +
                                   collision.collider.bounds.size.x) / 2f;

                if (normal.x > 0)
                    newPos.x = playerPos.x + halfWidth; // 오른쪽
                else
                    newPos.x = playerPos.x - halfWidth; // 왼쪽
            }
            else
            {
                // 상하 충돌일 경우 → y 위치 보정
                float yDiff = myPos.y - playerPos.y;
                float halfHeight = (GetComponent<Collider2D>().bounds.size.y +
                                    collision.collider.bounds.size.y) / 2f;

                if (normal.y > 0)
                    newPos.y = playerPos.y + halfHeight; // 아래에서 충돌 → 위에 붙이기
                else
                    newPos.y = playerPos.y - halfHeight; // 위에서 충돌 → 아래에 붙이기
            }

            // 위치 이동
            transform.position = newPos;
        }
    }

    //물체는 움직일 수 있는 물체여서 캐릭터가 shift키를 누르고 이 물체가 범위 안에 있다면 이 함수가 작동해서
    //hinge Joint2D에 플레이어가 가지고 있는 chain tail이랑 붙을예정임.
    public void LinkWithChainTail() 
    {

    }

}
