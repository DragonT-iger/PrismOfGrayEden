using UnityEngine;
using UnityEngine.UI;

//Press Any Key ∆‰¿Ã¡ˆ
public class OpeningAnyKey : MonoBehaviour
{
    public Button     btn_pressAnyKey;
    public GameObject img_SettingsPage;
    public GameObject img_Title;

    bool isClicked = false;
    void Start()
    {
        img_SettingsPage.SetActive(true);
        img_Title.SetActive(false);
    }

    private void Update()
    {
        if (!isClicked && Input.anyKeyDown)
        {
            img_SettingsPage.SetActive(false);
            img_Title.SetActive(true);
            print("GameSetting TItle On");
        }
    }
}
