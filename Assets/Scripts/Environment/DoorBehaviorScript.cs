using UnityEngine;
using NavMeshPlus.Components;

public class DoorBehaviorScript : MonoBehaviour, IPausable, ISortingOrderReceiver
{
    [SerializeField] private GameObject doorMask;
    [SerializeField] private ButtonScript buttonScript;

    private Animator animator;
    private Collider2D maskCollider;
    private NavMeshModifier maskModifier;
    // 소팅오더 기능 교정용
    private SpriteRenderer _spriteRenderer;

    private bool isPaused = true;
    private bool lastIsOpen;
    private float _previousAnimatorSpeed = 0f;

    NavMeshUpdater navMeshUpdater;

    public Vector2 VelocityBeforePause { get; private set; } = Vector2.zero;

    public bool IsPaused { get => isPaused; }

    void Start()
    {
        animator = GetComponent<Animator>();
        maskCollider = doorMask.GetComponent<Collider2D>();
        lastIsOpen = animator.GetBool("isOpen");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _previousAnimatorSpeed = animator.speed;
        navMeshUpdater = Object.FindFirstObjectByType<NavMeshUpdater>();
    }

    void Update()
    {
        // 일시정지 상태에 따라 애니메이터 스피드 설정
        float newSpeed = isPaused ? 0f : 1f;

        // 애니메이터 스피드가 변경되는 경우에만 OnAnimatorSpeedChanged 호출
        if (animator.speed != newSpeed)
        {
            float oldSpeed = animator.speed;
            animator.speed = newSpeed;
            OnAnimatorSpeedChanged(oldSpeed, newSpeed);
        }

        if (isPaused)
        {
            animator.speed = 0;
            return;
        }
        if (navMeshUpdater != null)
        {
            navMeshUpdater.RequestUpdate();
        }

        animator.speed = 1;
        bool currentIsOpen = buttonScript.isButtonOn;
        animator.SetBool("isOpen", currentIsOpen);

        if (currentIsOpen && !lastIsOpen)
        {
            SoundManager.Instance.PlaySFX("IronDoorOpen");
        }
        else if (!currentIsOpen && lastIsOpen)
        {
            SoundManager.Instance.PlaySFX("IronDoorOpen");
        }
        lastIsOpen = currentIsOpen;
        
        doorMask.transform.localPosition = new Vector3(0, transform.localPosition.y / 2, 0);
        doorMask.transform.localScale = new Vector3(4, 4 + transform.localPosition.y, 4);

        if (transform.localPosition.y <= -3.5f)
        {
            maskCollider.isTrigger = true;
        }
        else
        {
            maskCollider.isTrigger = false;
        }
    }

    // 애니메이터 스피드가 변경될 때 호출되는 메서드
    protected virtual void OnAnimatorSpeedChanged(float oldSpeed, float newSpeed)
    {
        // 스피드가 0이 되면 일시정지된 것
        if (newSpeed == 0f && oldSpeed > 0f)
        {
            SoundManager.Instance.PauseSFX(isPaused);
        }
        // 스피드가 0에서 다른 값으로 변경되면 재개된 것
        else if (newSpeed > 0f && oldSpeed == 0f)
        {
            SoundManager.Instance.PauseSFX(isPaused);
        }
    }


    public void OnSortingOrderChanged(int input)
    {
        _spriteRenderer.sortingOrder = input;
    }

    public void Pause() => isPaused = true;
    public void Resume() => isPaused = false;

    public void SetSavedVelocity(Vector2 input)
    {
        return;
    }
}
