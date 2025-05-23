using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class EnemyDetectionArea : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();

        // Ʈ���ŷ� ����
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // ���� �浹 ���� ������
        var rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == enemyAI.gameObject) return;

        var st = other.GetComponent<Status>();
        if (st != null)
            enemyAI.OnTargetEnteredDetectionArea(st);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == enemyAI.gameObject) return;

        var st = other.GetComponent<Status>();
        if (st != null)
            enemyAI.OnTargetExitedDetectionArea(st);
    }
}
