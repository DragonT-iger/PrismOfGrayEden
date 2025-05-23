using UnityEngine;

public class ObjectPause : MonoBehaviour, IPausable
{
    public Vector2 VelocityBeforePause { get; private set; }
    private bool isPaused;

    public bool IsPaused { get => isPaused; }

    float rotationBeforePause = 0f;

    public void Pause()
    {
        isPaused = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity != Vector2.zero)
        {
            VelocityBeforePause = rb.linearVelocity;
            rotationBeforePause = rb.angularVelocity;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
    public void Resume()
    {
        isPaused = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity == Vector2.zero && (VelocityBeforePause != default || rotationBeforePause != 0f))
        {
            rb.linearVelocity = VelocityBeforePause;
            rb.angularVelocity = rotationBeforePause;
            VelocityBeforePause = default;
            rotationBeforePause = 0f;
        }
    }

    public void SetSavedVelocity(Vector2 input)
    {
        VelocityBeforePause = input;
    }
}
