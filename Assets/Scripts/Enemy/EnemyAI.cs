// EnemyAI.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour, IPausable
{
    private float wanderRadius = 5f;
    private float wanderInterval = 2f;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;

    [Header("Dash Settings")]
    [Range(0f, 1f)] public float dashChance = 0.3f;
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;

    private string brokenFloorLayerName = "BrokenFloor";

    private SpriteRenderer spriteRenderer;

    bool isPaused = false;
    bool isDashing = false;
    float dashElapsed = 0f;
    Vector3 dashStartPos;
    Vector3 dashEndPos;

    NavMeshAgent agent;
    Rigidbody2D rb; // Rigidbody2D �߰�
    int brokenFloorLayer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
        brokenFloorLayer = LayerMask.NameToLayer(brokenFloorLayerName);
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Rigidbody2D�� isKinematic�� true�� �����Ͽ� ���������� ������ ���� �ʵ��� �մϴ�.
        rb.isKinematic = true;

        StartCoroutine(WanderRoutine());
    }

    // IPausable ����
    public void Pause()
    {
        isPaused = true;
        agent.isStopped = true;
    }

    public void Resume()
    {
        isPaused = false;
        if (!isDashing)
            agent.isStopped = false;
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            while (isPaused)
                yield return null;

            // ��� Ȯ�� üũ
            if (!isDashing && Random.value < dashChance)
            {
                if (RandomPointOnNavMesh(transform.position, wanderRadius, out Vector3 dashTarget))
                {
                    Vector3 dir = (dashTarget - transform.position).normalized;
                    yield return StartCoroutine(Dash(dir));
                }
            }
            else
            {
                // �Ϲ� ��ŷ
                if (RandomPointOnNavMesh(transform.position, wanderRadius, out Vector3 walkTarget))
                {
                    agent.SetDestination(walkTarget);
                    spriteRenderer.flipX = agent.velocity.x > 0.01f;
                }

                // Ŀ���� ��� (Pause ����)
                float timer = 0f;
                while (timer < wanderInterval)
                {
                    if (!isPaused)
                        timer += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }

    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        agent.isStopped = true;

        dashStartPos = transform.position;
        dashEndPos = dashStartPos + direction * dashDistance;
        dashElapsed = 0f;

        // ���� �ٴ� layer �浹 ����
        Physics2D.IgnoreLayerCollision(gameObject.layer, brokenFloorLayer, true);

        while (dashElapsed < dashDuration)
        {
            if (!isPaused)
            {
                dashElapsed += Time.deltaTime;
                float t = dashElapsed / dashDuration;
                transform.position = Vector3.Lerp(dashStartPos, dashEndPos, t);
            }
            yield return null;
        }

        transform.position = dashEndPos;
        Physics2D.IgnoreLayerCollision(gameObject.layer, brokenFloorLayer, false);

        isDashing = false;
        if (!isPaused)
            agent.isStopped = false;
    }

    bool RandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            randomPos.y = center.y;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = center;
        return false;
    }
}
