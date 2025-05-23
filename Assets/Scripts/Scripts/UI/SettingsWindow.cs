using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Play, Options, Quit 메뉴가 있는 곳, 추후 메뉴 추가될 예정

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private GameObject    ui_Title; //start, settings, quit 있는 화면
    public Button        btn_Settings;
    public Button        btn_GameQuit;
    public GameObject    OptionUI;
    private bool         bShowWndSettings;
    //public UIImageAnimator uiAnim;
    //public Button btn_pressAnyKey;
   // bool isClicked = false;

    //void Awake()
    //{
    //    img_SettingsPage.SetActive(true);
    //    img_Title.SetActive(false);
    //    print("Awake pre opening");
    //}

    void Start()
    {
        OptionUI.gameObject.SetActive(false); //이미지의 하위 오브젝트까지 모두 disabled
        
        btn_Settings.onClick.AddListener(OnClickOptions);
        
        btn_GameQuit.onClick.AddListener(OnClickQuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionUI.SetActive(false);
            ui_Title.SetActive(true);
            Debug.Log(ui_Title.name);
            SoundManager.Instance.PlaySFX("UIButtonClick");
        }
    }

    private void OnClickOptions()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            go.SetActive(false);
        }
        OptionUI.gameObject.SetActive(true);
        bShowWndSettings = false;
    }

    //설정 창 끄기
    public void OnClickQuitOptions()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            go.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape) || !bShowWndSettings)
        {
            OptionUI.gameObject.SetActive(true);
            bShowWndSettings = false;
        }
    }

    //게임종료
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
