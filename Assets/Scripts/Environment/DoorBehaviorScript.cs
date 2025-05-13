using UnityEngine;

public class DoorBehaviorScript : MonoBehaviour, IPausable
{
    [SerializeField] GameObject button;
    bool isPaused = false;
    public void Pause()
    {
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }

    void Update()
    {
        if (isPaused)
        {
            if (button.GetComponent<ButtonScript>().isButtonOn)
            {
                if (transform.localPosition.y <= 1.0f)
                {
                    transform.localPosition += new Vector3(0.0f, 0.5f, 0.0f) * Time.deltaTime;
                }
            }
            else
            {
                if (transform.localPosition.y >= 0.0f)
                {
                    transform.localPosition += new Vector3(0.0f, -0.5f, 0.0f) * Time.deltaTime;
                }
            }
        }
    }
}
