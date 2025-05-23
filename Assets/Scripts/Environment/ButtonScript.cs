using UnityEngine;

public class ButtonScript : MonoBehaviour, IPausable
{
    [SerializeField] float buttonDisableTime = 5.0f;
    [SerializeField] string[] buttonTriggerTag;

    bool isButtonPressed = false;
    public bool isButtonOn = false;
    float buttonPressedTime = 0.0f;
    bool isPaused = true;

    Animator animator;
    NavMeshUpdater navMeshUpdater;

    public Vector2 VelocityBeforePause { get; private set; } = Vector2.zero;

    public bool IsPaused { get => isPaused; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshUpdater = Object.FindFirstObjectByType<NavMeshUpdater>();
    }
    public void Pause()
    {
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < buttonTriggerTag.Length; i++)
        {
            if (collision.CompareTag(buttonTriggerTag[i]))
            {
                isButtonPressed = true;
                SoundManager.Instance.PlaySFX("ButtonDown");
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < buttonTriggerTag.Length; i++)
        {
            if (collision.CompareTag(buttonTriggerTag[i]))
            {
                isButtonPressed = false;
                break;
            }
        }
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            buttonPressedTime = 0.0f;
            isButtonOn = true;
            animator.SetBool("isButtonOn", isButtonOn);
        }
        else if (isPaused == false)
        {
            if (buttonPressedTime >= buttonDisableTime)
            {
                if (isButtonOn == true)
                {
                    SoundManager.Instance.PlaySFX("ButtonUp");
                }
                isButtonOn = false;
                animator.SetBool("isButtonOn", isButtonOn);
            }
            else
            {
                buttonPressedTime += Time.deltaTime;
            }
        }
    }

    public void SetSavedVelocity(Vector2 input)
    {
        return;
    }
}