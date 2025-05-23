using UnityEngine;

public class PressQActivefalseUI : MonoBehaviour
{

    public GameObject otherTitleUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
            SelectMenuWnd();
        }
    }

    public void SelectMenuWnd()
    {
        otherTitleUI.SetActive(true);
    }
}
