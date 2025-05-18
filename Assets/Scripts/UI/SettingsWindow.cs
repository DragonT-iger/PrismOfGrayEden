using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Play, Options, Quit 메뉴가 있는 곳, 추후 메뉴 추가될 예정

public class SettingsWindow : MonoBehaviour
{
    public Button        btn_Settings;
    public Button        btn_GameQuit;
    public GameObject    OptionUI;
    private bool         bShowWndSettings;


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
        }
    }

    private void OnClickOptions()
    {
        OptionUI.gameObject.SetActive(true);//true
        bShowWndSettings = false;
    }

    //설정 창 끄기
    public void OnClickQuitOptions()
    {
        if (Input.GetKey(KeyCode.Escape) || !bShowWndSettings)
        {
            Debug.Log("off ㅋㅋ");
            OptionUI.gameObject.SetActive(false);
            bShowWndSettings = false;
        }
    }

    //게임종료
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
