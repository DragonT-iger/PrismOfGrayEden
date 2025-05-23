using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Play, Options, Quit �޴��� �ִ� ��, ���� �޴� �߰��� ����

public class SettingsWindow : MonoBehaviour
{
    public Button        btn_Settings;
    public Button        btn_GameQuit;
    public GameObject    OptionUI;
    private bool         bShowWndSettings;


    void Start()
    {
        OptionUI.gameObject.SetActive(false); //�̹����� ���� ������Ʈ���� ��� disabled

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

    //���� â ����
    public void OnClickQuitOptions()
    {
        if (Input.GetKey(KeyCode.Escape) || !bShowWndSettings)
        {
            Debug.Log("off ����");
            OptionUI.gameObject.SetActive(false);
            bShowWndSettings = false;
        }
    }

    //��������
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
