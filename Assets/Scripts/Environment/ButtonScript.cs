using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] float buttneDisableTime = 5.0f;

    bool isButtonPressed = false;
    public bool isButtonOn = false;
    float buttonPressedTime = 0.0f;
    short buttonState = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isButtonPressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isButtonPressed = false;
        }
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            buttonPressedTime = 0.0f;
            isButtonOn = true;
            if (buttonState != 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                buttonState = 0;
            }
        }
        else
        {
            if (buttonPressedTime >= buttneDisableTime)
            {
                isButtonOn = false;
                if (buttonState != 2)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    buttonState = 2;
                }
            }
            else
            {
                buttonPressedTime += Time.deltaTime;
                if (buttonState != 1)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                    buttonState = 1;
                }
            }
        }
    }
}
