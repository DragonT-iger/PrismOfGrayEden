using UnityEngine;
using UnityEngine.AI;

public class PriestEnemy : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float talkDistance = 0.3f;

    [Header("Animation Settings")]
    [SerializeField] float directionPriority = 0.8f; // 상하 우선순위 (0.8 이상이면 상하 우선)

    private enum PriestState
    {
        Idle,
        Surprised,
        Moving,
        Talking,
        ReturnToIdle
    }

    private PriestState currentState = PriestState.Idle;
    private NavMeshAgent navAgent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private GameObject knight;

    // Animation parameters
    private int idleSpeedHash;
    private int walkSpeedHash;
    private int surpriseSpeedHash;
    private int talkSpeedHash;

    // Direction parameters
    private float currentXDirection = 0f;
    private float currentYDirection = 0f;
    private Vector3 lastPosition;

    // State management
    private float surpriseTimer = 0f;
    private float surpriseDuration = 2f; // Surprise 애니메이션 지속 시간
    private bool hasBeenSurprised = false;

    void Start() 
    {
        // 컴포넌트 초기화
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // NavMeshAgent 2D 설정
        if (navAgent != null)
        {
            navAgent.updateRotation = false;
            navAgent.updateUpAxis = false;
        }

        // 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 기사 찾기
        knight = GameObject.FindGameObjectWithTag("Knight");

        // Animation parameter 해시 값 저장 (성능 최적화)
        idleSpeedHash = Animator.StringToHash("IdleSpeed");
        walkSpeedHash = Animator.StringToHash("WalkSpeed");
        surpriseSpeedHash = Animator.StringToHash("SurpriseSpeed");
        talkSpeedHash = Animator.StringToHash("TalkSpeed");

        lastPosition = transform.position;

        // 초기 상태 설정
        SetState(PriestState.Idle);
    }

    void Update() {
        UpdateStateMachine();
        UpdateAnimation();
    }

    void UpdateStateMachine() {
        switch (currentState)
        {
            case PriestState.Idle:
                HandleIdleState();
                break;

            case PriestState.Surprised:
                HandleSurprisedState();
                break;

            case PriestState.Moving:
                HandleMovingState();
                break;

            case PriestState.Talking:
                HandleTalkingState();
                break;

            case PriestState.ReturnToIdle:
                HandleReturnToIdleState();
                break;
        }
    }

    void HandleIdleState() {
        if (player != null && !hasBeenSurprised)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                SetState(PriestState.Surprised);
                hasBeenSurprised = true;
            }
        }
    }

    void HandleSurprisedState() {
        surpriseTimer += Time.deltaTime;

        if (surpriseTimer >= surpriseDuration)
        {
            surpriseTimer = 0f;

            if (knight != null)
            {
                SetState(PriestState.Moving);
                navAgent.SetDestination(knight.transform.position);
            }
            else
            {
                SetState(PriestState.ReturnToIdle);
            }
        }
    }

    void HandleMovingState() {
        if (knight != null)
        {
            float distanceToKnight = Vector3.Distance(transform.position, knight.transform.position);

            // 기사와의 거리가 충분히 가까우면 대화 상태로 전환
            if (distanceToKnight <= talkDistance)
            {
                navAgent.ResetPath();
                SetState(PriestState.Talking);
            }
            else
            {
                // 목적지 업데이트 (기사가 움직일 수 있으므로)
                navAgent.SetDestination(knight.transform.position);
            }
        }
        else
        {
            SetState(PriestState.ReturnToIdle);
        }
    }

    void HandleTalkingState() 
    {
        if (knight != null)
        {
            // 기사 방향을 바라보도록 방향 설정
            Vector3 directionToKnight = (knight.transform.position - transform.position).normalized;
            UpdateDirectionToTarget(directionToKnight);

            // 기사가 너무 멀어지면 다시 이동
            float distanceToKnight = Vector3.Distance(transform.position, knight.transform.position);
            if (distanceToKnight > talkDistance * 2f)
            {
                SetState(PriestState.Moving);
                navAgent.SetDestination(knight.transform.position);
            }
        }
        else
        {
            SetState(PriestState.ReturnToIdle);
        }
    }

    void HandleReturnToIdleState() {
        // 잠시 대기 후 Idle로 복귀
        SetState(PriestState.Idle);
    }

    void SetState(PriestState newState) {
        currentState = newState;

        // 상태별 애니메이션 파라미터 설정
        ResetAllAnimationSpeeds();

        switch (newState)
        {
            case PriestState.Idle:
                animator.SetFloat(idleSpeedHash, 1f);
                break;

            case PriestState.Surprised:
                animator.SetFloat(surpriseSpeedHash, 1f);
                break;

            case PriestState.Moving:
                animator.SetFloat(walkSpeedHash, 1f);
                break;

            case PriestState.Talking:
                animator.SetFloat(talkSpeedHash, 1f);
                break;

            case PriestState.ReturnToIdle:
                animator.SetFloat(idleSpeedHash, 1f);
                break;
        }
    }

    void ResetAllAnimationSpeeds() 
    {
        animator.SetFloat(idleSpeedHash, 0f);
        animator.SetFloat(walkSpeedHash, 0f);
        animator.SetFloat(surpriseSpeedHash, 0f);
        animator.SetFloat(talkSpeedHash, 0f);
    }

    void UpdateAnimation() 
    {
        // 이동 중일 때 방향 업데이트
        if (currentState == PriestState.Moving)
        {
            Vector3 movementDirection = transform.position - lastPosition;
            if (movementDirection.magnitude > 0.01f)
            {
                UpdateDirectionToTarget(movementDirection.normalized);
            }
        }

        // 애니메이터에 방향 값 전달
        animator.SetFloat("xDirection", currentXDirection);
        animator.SetFloat("yDirection", currentYDirection);

        // 스프라이트 플립 처리
        UpdateSpriteFlip();

        lastPosition = transform.position;
    }

    void UpdateDirectionToTarget(Vector3 direction) 
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        // 상하 우선순위 적용
        if (absY >= directionPriority || absY > absX)
        {
            // 상하 방향 우선
            currentXDirection = 0f;
            currentYDirection = direction.y > 0 ? 1f : -1f;
        }
        else
        {
            // 좌우 방향
            currentXDirection = direction.x > 0 ? 1f : -1f;
            currentYDirection = 0f;
        }
    }

    void UpdateSpriteFlip() 
    {
        // 좌우 방향일 때만 플립 적용
        if (Mathf.Abs(currentXDirection) > 0.1f)
        {
            spriteRenderer.flipX = currentXDirection < 0;
        }
    }

 /*   // 외부에서 상태 확인용
    public PriestState GetCurrentState() 
    {
        return currentState;
    }
*/
    // 강제로 놀라게 하기 (디버그용)
    public void ForceSurprise() 
    {
        if (currentState == PriestState.Idle)
        {
            SetState(PriestState.Surprised);
            hasBeenSurprised = true;
        }
    }

    // 리셋 (재사용시)
    public void ResetPriest() 
    {
        hasBeenSurprised = false;
        surpriseTimer = 0f;
        navAgent.ResetPath();
        SetState(PriestState.Idle);
    }
}
