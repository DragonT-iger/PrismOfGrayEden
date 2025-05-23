using UnityEngine;

public class FirePause : MonoBehaviour, IPausable
{
    Material fireMaterial;

    private Vector2 velocityBeforePause;
    private bool isPaused = true;
    private float savedCustomTime = 0f;
    private float customTime = 0f;

    public Vector2 VelocityBeforePause => velocityBeforePause;
    public bool IsPaused => isPaused;

    void Start()
    {
        fireMaterial = GetComponent<Renderer>().material;
        if (fireMaterial != null)
        {
            customTime = fireMaterial.GetFloat("_CustomTime");
        }
    }

    void Update()
    {
        if (!isPaused && fireMaterial != null)
        {
            customTime += Time.deltaTime;
            fireMaterial.SetFloat("_CustomTime", customTime);
        }
    }

    public void Pause()
    {
        isPaused = true;
        if (fireMaterial != null)
        {
            savedCustomTime = customTime;
        }
    }

    public void Resume()
    {
        isPaused = false;
        if (fireMaterial != null)
        {
            customTime = savedCustomTime;
        }
    }

    public void SetSavedVelocity(Vector2 input)
    {
        velocityBeforePause = input;
    }
}
