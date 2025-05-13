using UnityEngine;

public class ObjectPause : MonoBehaviour, IPausable
{
    Vector2 velocityBeforePause = default;
    float rotationBeforePause = 0f;

    public void Pause()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity != Vector2.zero)
        {
            velocityBeforePause = rb.linearVelocity;
            rotationBeforePause = rb.angularVelocity;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
    public void Resume()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.linearVelocity == Vector2.zero && (velocityBeforePause != default || rotationBeforePause != 0f))
        {
            rb.linearVelocity = velocityBeforePause;
            rb.angularVelocity = rotationBeforePause;
            velocityBeforePause = default;
            rotationBeforePause = 0f;
        }
    }
}
