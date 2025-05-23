using UnityEngine;

public class PressActivefalseUI : MonoBehaviour
{

    public GameObject otherTitleUI;
    void Update()
    {
        if (Input.anyKeyDown)
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
