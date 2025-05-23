// EnemyDetectionAreaGizmo.cs
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyDetectionAreaGizmo : MonoBehaviour
{
    [Header("Gizmo ǥ�� ����")]
    public bool showGizmo = true;
    [Header("Gizmo ����")]
    public Color gizmoColor = Color.red;

    private CircleCollider2D col;
    private EnemyAI enemyAI;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        if (col == null) col = GetComponent<CircleCollider2D>();
        if (enemyAI == null) enemyAI = GetComponentInParent<EnemyAI>();

        // �ݰ�: ������ �ݿ�
        float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        float radius = col.radius * scale;
        float halfAngle = enemyAI.viewAngle * 0.5f;

        // �� flip ���¿� ���� forward ����
        Vector2 forward = enemyAI.GetComponent<SpriteRenderer>().flipX
                          ? Vector2.left
                          : Vector2.right;

        // �þ߼� ���
        Vector3 dirA = Quaternion.Euler(0, 0, halfAngle) * forward;
        Vector3 dirB = Quaternion.Euler(0, 0, -halfAngle) * forward;

        Gizmos.color = gizmoColor;
        // �þ� ��
        Gizmos.DrawLine(transform.position, transform.position + dirA * radius);
        Gizmos.DrawLine(transform.position, transform.position + dirB * radius);
    }
}
