using UnityEngine;

public class MoveableObj : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && false) // false�� shift Ű �Է� �޴� ���� �ٷ� chain�� ���� 
        {
            Transform player = collision.gameObject.transform;
            transform.SetParent(player); // �θ�� ����

            Vector2 normal = collision.contacts[0].normal;
            Vector3 playerPos = player.position;
            Vector3 myPos = transform.position;

            Vector3 newPos = myPos;

            // �ε��� ���⿡ ���� �ش� ���� ��ġ�� ����
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                // �¿� �浹�� ��� �� x ��ġ ����
                float xDiff = myPos.x - playerPos.x;
                float halfWidth = (GetComponent<Collider2D>().bounds.size.x +
                                   collision.collider.bounds.size.x) / 2f;

                if (normal.x > 0)
                    newPos.x = playerPos.x + halfWidth; // ������
                else
                    newPos.x = playerPos.x - halfWidth; // ����
            }
            else
            {
                // ���� �浹�� ��� �� y ��ġ ����
                float yDiff = myPos.y - playerPos.y;
                float halfHeight = (GetComponent<Collider2D>().bounds.size.y +
                                    collision.collider.bounds.size.y) / 2f;

                if (normal.y > 0)
                    newPos.y = playerPos.y + halfHeight; // �Ʒ����� �浹 �� ���� ���̱�
                else
                    newPos.y = playerPos.y - halfHeight; // ������ �浹 �� �Ʒ��� ���̱�
            }

            // ��ġ �̵�
            transform.position = newPos;
        }
    }

    //��ü�� ������ �� �ִ� ��ü���� ĳ���Ͱ� shiftŰ�� ������ �� ��ü�� ���� �ȿ� �ִٸ� �� �Լ��� �۵��ؼ�
    //hinge Joint2D�� �÷��̾ ������ �ִ� chain tail�̶� ����������.
    public void LinkWithChainTail() 
    {

    }

}
