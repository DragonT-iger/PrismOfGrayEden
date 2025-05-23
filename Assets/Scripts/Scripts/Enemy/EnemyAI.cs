// EnemyAI.cs
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour, IPausable
{
    public enum TargetTag { Knight, Furry, Priest, Player }

    /* ────────────── 인스펙터 설정 ────────────── */
    [Header("우선순위 태그 (1 → 3)")]
    [SerializeField] private TargetTag priority1 = TargetTag.Knight;
    [SerializeField] private TargetTag priority2 = TargetTag.Furry;
    [SerializeField] private TargetTag priority3 = TargetTag.Priest;

    [Header("Detection 설정")]
    [SerializeField] private LayerMask detectionMask;
    [SerializeField, Range(0, 360)] public float viewAngle = 100f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("공격 설정")]
    [SerializeField] private float attackInterval = 1f;         // 공격 주기
    [SerializeField] private int attackDamage = 10;          // 데미지
    [SerializeField] private float attackAnimationDuration = 0.3f;

    [Header("Dash 설정")]
    [SerializeField] private float dashDistance = 2f;           // 지나칠 거리
    [SerializeField] private float dashDuration = 0.2f;         // 대쉬 시간

    [Header("기사/사제")]
    [SerializeField] private bool isPriest = false;
    [SerializeField] private GameObject linkedKnight;

    /* ────────────── 컴포넌트 캐시 ────────────── */
    NavMeshAgent agent;
    SpriteRenderer sprite;
    Animator ani;

    /* ────────────── Detection/Attack 영역 ────────────── */
    CircleCollider2D detectionCol;
    Transform detectionRoot;
    CircleCollider2D attackCol;
    Transform attackRoot;

    /* ────────────── 상태 변수 ────────────── */
    public Transform currentTarget;
    public Status attackTarget;
    bool inAttackRange;
    bool isAttack;
    bool isDashing;
    float attackTimer;
    Vector2 targetDir;
    float walkAudioPlayRate = 1.5f;
    float walkAudioTimer = 0f;
    bool priestPlayerFound = false;
    Transform playerFoundPos;
    int lastIndex = -1; // 사운드 랜덤
    /* ────────────── 일시정지 ────────────── */
    bool isPaused = true;

    /* ────────────── Animator 파라미터 ────────────── */
    readonly string idleSpeedParam = "IdleSpeed";
    readonly string walkSpeedParam = "WalkSpeed";
    readonly string attackSpeedParam = "AttackSpeed";
    readonly string xDirParam = "xDirection";
    readonly string yDirParam = "yDirection";

    public Vector2 VelocityBeforePause { get; private set; } = Vector2.zero;

    public bool IsPaused { get => isPaused; }

    /* ────────────── Unity ────────────── */
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        ani.speed = 0f;

        // 자식 CircleCollider2D 검색
        foreach (var col in GetComponentsInChildren<CircleCollider2D>())
        {
            if (col.name == "EnemyDetectionArea")
            {
                detectionCol = col;
                detectionRoot = col.transform;
                col.isTrigger = true;
            }
            else if (col.name == "AttackArea")
            {
                attackCol = col;
                attackRoot = col.transform;
                col.isTrigger = true;
            }
        }

        if (!detectionCol) Debug.LogWarning("EnemyDetectionArea 없음");
        if (!attackCol) Debug.LogWarning("AttackArea 없음");
    }

    void Update()
    {
        if (isPaused) return;


        /* ── 대쉬 중 ── */
        if (isDashing)
        {
            UpdateAnimWhileDash();
            return;
        }

        walkAudioTimer += Time.deltaTime;
        /*if (walkAudioTimer >= walkAudioPlayRate)
        {
            SoundManager.Instance.PlaySFX("KnightWalk");
            walkAudioTimer = 0f;
        }*/
        /* ── 일반 이동/추적 ── */
        if (agent.velocity.sqrMagnitude > 0.001f)
            sprite.flipX = agent.velocity.x < 0f;

        UpdateTargetDir();

        if (!isPriest && inAttackRange && attackTarget)
        {
            agent.isStopped = true;
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                StartCoroutine(DashAndAttack());
            }
        }
        else if (!priestPlayerFound)
        {
            if (!isPriest)
            {
                agent.isStopped = false;
            }
            ChaseLogic();
        }
        else if (isPriest)
        {
            agent.isStopped = false;
            priority1 = TargetTag.Knight;
            priority2 = TargetTag.Knight;
            priority3 = TargetTag.Knight;
            if (FindPriorityTarget())
            {
                linkedKnight.GetComponent<EnemyAI>().currentTarget = playerFoundPos;
                priority1 = TargetTag.Player;
                priority2 = TargetTag.Player;
                priority3 = TargetTag.Player;
                agent.isStopped = true;
                priestPlayerFound = false;
            }
        }
        UpdateAnimParameters();
    }

    /* ────────────── 타겟 탐색/추적 ────────────── */
    void UpdateTargetDir()
    {
        if (currentTarget)
            targetDir = (currentTarget.position - transform.position).normalized;
        else if (attackTarget)
            targetDir = (attackTarget.transform.position - transform.position).normalized;
    }

    void ChaseLogic()
    {
        var next = FindPriorityTarget();
        if (!isPriest && next) currentTarget = next;
        if (isPriest && next)
        {
            agent.isStopped = false;
            playerFoundPos = next;
            currentTarget = linkedKnight.transform;
            priestPlayerFound = true;
        }
        if (currentTarget)
            agent.SetDestination(currentTarget.position);
    }

    Transform FindPriorityTarget()
    {
        if (!detectionCol) return null;

        Vector2 origin = detectionRoot.position;
        float radius = detectionCol.radius * Mathf.Max(detectionRoot.lossyScale.x, detectionRoot.lossyScale.y);
        float half = viewAngle * 0.5f;
        Vector2 forward = sprite.flipX ? Vector2.left : Vector2.right;

        var hits = Physics2D.OverlapCircleAll(origin, radius, detectionMask);
        foreach (var tag in new[] { priority1, priority2, priority3 })
        {
            Transform best = null; float minSqr = float.MaxValue;
            foreach (var h in hits)
            {
                if (!h.CompareTag(tag.ToString())) continue;
                Vector2 dir = ((Vector2)h.transform.position - origin).normalized;
                if (Vector2.Angle(forward, dir) > half) continue;
                if (Physics2D.Raycast(origin, dir, radius, obstacleMask)) continue;
                float ds = ((Vector2)h.transform.position - origin).sqrMagnitude;
                if (ds < minSqr) { minSqr = ds; best = h.transform; }
            }
            if (best) return best;
        }
        return null;
    }

    /* ────────────── Detection/Attack 콜백 ────────────── */
    public void OnTargetEnteredDetectionArea(Status t)
    {
        foreach (var tag in new[] { priority1, priority2, priority3 })
        {
            if (t.CompareTag(tag.ToString()) && t.currentHP >= 0)
            {
                currentTarget = t.transform;          // 즉시 공격
            }
        }
    }
    public void OnTargetExitedDetectionArea(Status t) { if (currentTarget == t.transform) currentTarget = null; }

    public void OnTargetEnteredAttackArea(Status t)
    {
        foreach (var tag in new[] { priority1, priority2, priority3 })
        {
            if (t.CompareTag(tag.ToString()) && t.currentHP >= 0)
            {
                attackTarget = t;
                inAttackRange = true;
                attackTimer = attackInterval;           // 즉시 공격
            }
        }
    }
    public void OnTargetExitedAttackArea(Status t)
    {
        if (attackTarget == t)
        {
            attackTarget = null;
            inAttackRange = false;
        }
    }


    public void NotifyTargetDied(Status s) 
    {
        if (attackTarget == s)
        {
            attackTarget = null;
            inAttackRange = false;
        }

        if (currentTarget == s.transform)
        {
            currentTarget = null;
        }
    }







    /* ────────────── Dash + Attack ────────────── */
    IEnumerator DashAndAttack()
    {
        SoundManager.Instance.PlaySFX("KnightAttack");
        Status tgt = attackTarget;
        if (tgt == null) yield break;
        if (!tgt) yield break;
        if (tgt.currentHP <= 0) yield break;

        isDashing = true;
        agent.isStopped = true;

        Vector3 start = transform.position;
        Vector2 dir = (tgt.transform.position - start).normalized;

        /* 대쉬 직전에 블렌드 트리 방향 고정 */
        SetDirectionParams(dir);

        float speed = dashDistance / dashDuration;
        float elapsed = 0f;

        int tgtLayerBit = 1 << tgt.gameObject.layer;
        int maskWithoutTgt = obstacleMask.value & ~tgtLayerBit;

        while (elapsed < dashDuration)
        {
            if (isPaused) { yield return null; continue; }

            float step = speed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, step, maskWithoutTgt);
            if (hit.collider)
            {
                transform.position = hit.point;
                break;
            }
            transform.position += (Vector3)(dir * step);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (elapsed >= dashDuration)
            transform.position = start + (Vector3)(dir * dashDistance);

        /* ── 공격 애니메이션 & 데미지 적용 ── */
        isAttack = true;
        ani.SetFloat(attackSpeedParam, 1f);

        yield return new WaitForSeconds(attackAnimationDuration);

        tgt.TakeDamage(attackDamage);
        SoundManager.Instance.PlaySFX("PlayerHitKnight");

        ani.SetFloat(attackSpeedParam, 0f);
        isAttack = false;
        isDashing = false;
        agent.isStopped = false;
    }

    /* ────────────── Animator 업데이트 ────────────── */
    void UpdateAnimWhileDash()
    {
        ani.SetFloat(idleSpeedParam, 0f);
        ani.SetFloat(walkSpeedParam, 1f);
        ani.SetFloat(attackSpeedParam, 0f);
        // 방향은 Dash 시작 시 고정했으므로 그대로 둠
    }

    void UpdateAnimParameters()
    {
        if (isAttack) return;

        // “움직이고 있다” 판정
        bool moving = !agent.isStopped &&
                      (agent.desiredVelocity.sqrMagnitude > 0.01f || agent.velocity.sqrMagnitude > 0.01f);

        // damping 을 줘서 1-프레임 스파이크 완화
        ani.SetFloat(idleSpeedParam, moving ? 0f : 1f, 0.1f, Time.deltaTime);
        ani.SetFloat(walkSpeedParam, moving ? 1f : 0f, 0.1f, Time.deltaTime);

        if (moving)
            SetDirectionParams(agent.desiredVelocity);   // desiredVelocity 로 방향 고정
    }

    /* ────────────── 방향 파라미터 세팅 ────────────── */
    void SetDirectionParams(Vector2 dir)
    {
        dir.Normalize();
        ani.SetFloat(xDirParam, dir.x);
        ani.SetFloat(yDirParam, dir.y);

        if (Mathf.Abs(dir.x) > 0.01f)
            sprite.flipX = dir.x < 0f;
    }

    /* ────────────── IPausable ────────────── */
    public void Pause()
    {
        ani.speed = 0f;
        isPaused = true;
        agent.isStopped = true;
    }
    public void Resume()
    {
        ani.speed = 1f;
        isPaused = false;
    }

    public void SetSavedVelocity(Vector2 input)
    {
        return;
    }
    // ------------- Sound ------------------- //
    public void EnemyWalkSFX() 
    {
        if (gameObject.CompareTag("Furry"))
        {
            // 울프 사운드 
            SoundManager.Instance.PlaySFX("KnightWalk1");
        }
        else
        {
            int newIndex = GetRandomIndexExcludingLast(4, lastIndex);
            lastIndex = newIndex;

            string clipName = "KnightWalk" + (newIndex + 1).ToString(); // "KnightWalk1" ~ "KnightWalk4"
            SoundManager.Instance.PlaySFX(clipName);
            //SoundManager.Instance.PlaySFX("KnightWalk1");
            Debug.Log(clipName);
        }
    }
    int GetRandomIndexExcludingLast(int count, int last)
    {
        if (count <= 1) return 0;

        int rand = Random.Range(0, count - 1);
        if (rand >= last)
            rand++;

        return rand;
    }



}