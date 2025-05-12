using UnityEngine;

public class ObjectPause : MonoBehaviour, IPausable
{
    Vector2 velocityBeforePause;

    public void Pause()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity != Vector2.zero)
        {
            velocityBeforePause = rb.linearVelocity;
            rb.linearVelocity = Vector2.zero;
        }
    }
    public void Resume()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity == Vector2.zero && velocityBeforePause != default)
        {
            rb.linearVelocity = velocityBeforePause;
            velocityBeforePause = default;
        }
    }
}
