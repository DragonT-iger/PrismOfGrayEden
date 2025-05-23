using UnityEngine;
using UnityEngine.UI;

//Press Any Key ∆‰¿Ã¡ˆ
public class OpeningAnyKey : MonoBehaviour
{
    public Button     btn_pressAnyKey;
    public GameObject img_SettingsPage;
    public GameObject img_Title;
    public UIImageAnimator uiAnim;

    bool istemp = false;

    bool isClicked = false;
    void Awake()
    {
        img_SettingsPage.SetActive(true);
        img_Title.SetActive(false);
        print("Awake pre opening");
    }

    private void Update()
    {
        if (!isClicked && Input.anyKeyDown && !istemp)
        {
            img_Title.SetActive(true);
            img_SettingsPage.SetActive(false);
            print("GameSetting TItle On");
            istemp = true;
            SoundManager.Instance.PlaySFX("UIButtonClick");
        }
    }
}
