using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMain : MonoBehaviour
{
    [SerializeField] float exitDelay = 50.0f;
    float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= exitDelay)
        {
            SceneManager.Instance.LoadScene("MainMenu");
        }
    }
}
