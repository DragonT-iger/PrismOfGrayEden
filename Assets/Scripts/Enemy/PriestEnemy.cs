using UnityEngine;
using UnityEngine.AI;

public class PriestEnemy : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float talkDistance = 0.3f;

    [Header("Animation Settings")]
    [SerializeField] float directionPriority = 0.8f; // ���� �켱���� (0.8 �̻��̸� ���� �켱)

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
    private float surpriseDuration = 2f; // Surprise �ִϸ��̼� ���� �ð�
    private bool hasBeenSurprised = false;

    void Start() 
    {
        // ������Ʈ �ʱ�ȭ
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // NavMeshAgent 2D ����
        if (navAgent != null)
        {
            navAgent.updateRotation = false;
            navAgent.updateUpAxis = false;
        }

        // �÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // ��� ã��
        knight = GameObject.FindGameObjectWithTag("Knight");

        // Animation parameter �ؽ� �� ���� (���� ����ȭ)
        idleSpeedHash = Animator.StringToHash("IdleSpeed");
        walkSpeedHash = Animator.StringToHash("WalkSpeed");
        surpriseSpeedHash = Animator.StringToHash("SurpriseSpeed");
        talkSpeedHash = Animator.StringToHash("TalkSpeed");

        lastPosition = transform.position;

        // �ʱ� ���� ����
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

            // ������ �Ÿ��� ����� ������ ��ȭ ���·� ��ȯ
            if (distanceToKnight <= talkDistance)
            {
                navAgent.ResetPath();
                SetState(PriestState.Talking);
            }
            else
            {
                // ������ ������Ʈ (��簡 ������ �� �����Ƿ�)
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
            // ��� ������ �ٶ󺸵��� ���� ����
            Vector3 directionToKnight = (knight.transform.position - transform.position).normalized;
            UpdateDirectionToTarget(directionToKnight);

            // ��簡 �ʹ� �־����� �ٽ� �̵�
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
        // ��� ��� �� Idle�� ����
        SetState(PriestState.Idle);
    }

    void SetState(PriestState newState) {
        currentState = newState;

        // ���º� �ִϸ��̼� �Ķ���� ����
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
        // �̵� ���� �� ���� ������Ʈ
        if (currentState == PriestState.Moving)
        {
            Vector3 movementDirection = transform.position - lastPosition;
            if (movementDirection.magnitude > 0.01f)
            {
                UpdateDirectionToTarget(movementDirection.normalized);
            }
        }

        // �ִϸ����Ϳ� ���� �� ����
        animator.SetFloat("xDirection", currentXDirection);
        animator.SetFloat("yDirection", currentYDirection);

        // ��������Ʈ �ø� ó��
        UpdateSpriteFlip();

        lastPosition = transform.position;
    }

    void UpdateDirectionToTarget(Vector3 direction) 
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        // ���� �켱���� ����
        if (absY >= directionPriority || absY > absX)
        {
            // ���� ���� �켱
            currentXDirection = 0f;
            currentYDirection = direction.y > 0 ? 1f : -1f;
        }
        else
        {
            // �¿� ����
            currentXDirection = direction.x > 0 ? 1f : -1f;
            currentYDirection = 0f;
        }
    }

    void UpdateSpriteFlip() 
    {
        // �¿� ������ ���� �ø� ����
        if (Mathf.Abs(currentXDirection) > 0.1f)
        {
            spriteRenderer.flipX = currentXDirection < 0;
        }
    }

 /*   // �ܺο��� ���� Ȯ�ο�
    public PriestState GetCurrentState() 
    {
        return currentState;
    }
*/
    // ������ ���� �ϱ� (����׿�)
    public void ForceSurprise() 
    {
        if (currentState == PriestState.Idle)
        {
            SetState(PriestState.Surprised);
            hasBeenSurprised = true;
        }
    }

    // ���� (�����)
    public void ResetPriest() 
    {
        hasBeenSurprised = false;
        surpriseTimer = 0f;
        navAgent.ResetPath();
        SetState(PriestState.Idle);
    }
}
