using UnityEngine;

public class DoorBehaviorScript : MonoBehaviour
{
    [SerializeField] GameObject button;

    void Update()
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
