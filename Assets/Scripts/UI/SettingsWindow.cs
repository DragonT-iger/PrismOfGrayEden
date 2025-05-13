using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    public Button   btn_Settings;
    public Button   btn_GameStart;
    public Button   btn_GameQuit;
    public Button   btn_QuitSettingWnd;
    public Button   btn_Return;

    public Image    img_SettingsBackground;
    public Image    img_SettingsWnd;
    public Slider   sl_EffectSound;
    public Slider   sl_BackgroundSound;
    private bool    bShowWndSettings;

    int MAX_SOUND_VALUE = 1;


    void Start()
    {
        img_SettingsBackground.gameObject.SetActive(false); //�̹����� ���� ������Ʈ���� ��� enabled

        btn_Settings.onClick.AddListener(OnClickSettingsButton);
        btn_QuitSettingWnd.onClick.AddListener(OnClickQuitSettingWnd);
        btn_GameQuit.onClick.AddListener(OnClickQuitGame);
        btn_GameStart.onClick.AddListener(OnClickStartGame);
        btn_Return.onClick.AddListener(OnClickBacktoMenu);

        sl_EffectSound.onValueChanged.AddListener(OnValueChaged_EffectSound);
        sl_BackgroundSound.onValueChanged.AddListener(OnValueChaged_BackgroundSound);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) || !bShowWndSettings)
        {
            img_SettingsBackground.gameObject.SetActive(false);
            bShowWndSettings = false;
        }
    }

    private void OnClickSettingsButton()
    {
        if(!bShowWndSettings)
        {
            img_SettingsBackground.gameObject.SetActive(true);
            bShowWndSettings = true;
        }
    }

    //ȿ���� ����
    public void OnValueChaged_EffectSound(float value)
    {
        float volume = value / (float)MAX_SOUND_VALUE;
    }

    //������� ����
    public void OnValueChaged_BackgroundSound(float value)
    {
        float volume = value / (float)MAX_SOUND_VALUE;
    }

    //���� â ����
    public void OnClickQuitSettingWnd()
    {
        img_SettingsBackground.gameObject.SetActive(false);
        bShowWndSettings = false;
    }

    //���Ӿ����� �Ѿ��
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("MainMenu 1");
    }

    //��������
    public void OnClickQuitGame()
    {
        Application.Quit();
    }

    //���� ȭ������ �Ѿ��
    public void OnClickBacktoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
