using UnityEngine;

// IPausable.cs
public interface IPausable
{
    Vector2 VelocityBeforePause {get;}

    bool IsPaused {get;}

    void Pause();
    void Resume();

    void SetSavedVelocity(Vector2 input);
}
