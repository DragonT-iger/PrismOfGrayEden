using UnityEngine;
using NavMeshPlus.Components;

public class DoorBehaviorScript : MonoBehaviour, IPausable, ISortingOrderReceiver
{
    [SerializeField] private GameObject doorMask;
    [SerializeField] private ButtonScript buttonScript;

    private Animator animator;
    private Collider2D maskCollider;
    private NavMeshModifier maskModifier;
    // ���ÿ��� ��� ������
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
        // �Ͻ����� ���¿� ���� �ִϸ����� ���ǵ� ����
        float newSpeed = isPaused ? 0f : 1f;

        // �ִϸ����� ���ǵ尡 ����Ǵ� ��쿡�� OnAnimatorSpeedChanged ȣ��
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

    // �ִϸ����� ���ǵ尡 ����� �� ȣ��Ǵ� �޼���
    protected virtual void OnAnimatorSpeedChanged(float oldSpeed, float newSpeed)
    {
        // ���ǵ尡 0�� �Ǹ� �Ͻ������� ��
        if (newSpeed == 0f && oldSpeed > 0f)
        {
            SoundManager.Instance.PauseSFX(isPaused);
        }
        // ���ǵ尡 0���� �ٸ� ������ ����Ǹ� �簳�� ��
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
