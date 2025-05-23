using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Options Page

// 1) �ɼ� ������ enum���� ����
public enum OptionType
{
    ControlGuide,
    DisplayMode,
    DisplayResolution,
    Sound,
    Language,
    LoadCheckpoint,
    Credit,
    BackToTitle,
}
[System.Serializable]
public class Grid{
    public GameObject name;
    public GameObject[] contents;
}

public class OptionSettingsCtrl : MonoBehaviour
{
    public Slider   sl_EffectSound;
    public Slider   sl_BackgroundSound;
    public GameObject txt_optionTitle;//settings Ŭ�� �� �߻��ϴ� optionâ�� Ÿ��Ʋ �ؽ�Ʈ
    public GameObject img_OptionsPage;
    public GameObject grid_ControlGuide;
    public GameObject img_Keys;       //Ű�е� ���� �̹���
    public GameObject grid_DisplayMode;
    public GameObject grid_DisplayResolution;
    public GameObject grid_SoundVolume;
    public GameObject grid_ScrollView;
    public GameObject grid_Credits;

    int MAX_SOUND_VALUE = 1;

    // 2) Inspector���� ������� ��ư�� �Ҵ� (enum ������ �����ϰ�)
    [SerializeField] private GameObject[] optionButtons;

    [SerializeField] private Grid[] option;

    [SerializeField] private GameObject txt_options;

    private void Awake()
    {
        // enum ������ �迭 ���� ���߱� üũ
        var enumCount = System.Enum.GetNames(typeof(OptionType)).Length;
        if (optionButtons.Length != enumCount)
        {
            Debug.Log($"OptionSettingsCtrl: ��ư �迭 ����({optionButtons.Length})�� OptionType ����({enumCount})�� �ٸ��ϴ�.");
        }

        //for(int i = 0; i < optionButtons.Length; i++)
        //{
        //    optionButtons[i].SetActive(true); 
        //}
    }

    public void Start()
    {
        img_OptionsPage.gameObject.SetActive(false); //��ü ���� ������Ʈ���� ��� disabled
        txt_optionTitle.gameObject.SetActive(true); //�ɼ� â�� Ÿ��Ʋ �ؽ�Ʈ options Ȱ��ȭ
        optionButtons[(int)OptionType.ControlGuide].SetActive(true); //�ɼ�â �߸� �⺻������ ctrl guide�� �ߵ���
    }

    // 4) Ŭ�� �� ȣ��Ǵ� ���� �޼���
    public void OnClickOption(OptionType option)
    {
        // ��: Ŭ���� �ɼ� �̸��� �ؽ�Ʈ�� ǥ��
        txt_options.gameObject.name = $"���õ� �ɼ�: {option}";
        
        switch (option)
        {
            case OptionType.ControlGuide:
                ShowControlGuide(option);
                break;
            case OptionType.DisplayMode:
                ToggleDisplayMode(option);
                break;
            case OptionType.DisplayResolution:
                ChangeResolution(option);
                break;
            case OptionType.Sound:
                OpenSoundSettings(option);
                break;
            case OptionType.Language:
                OpenLanguageSettings(option);
                break;
            case OptionType.LoadCheckpoint:
                LoadLastCheckpoint(option);
                break;
            case OptionType.Credit:
                ShowCredits(option);
                break;
            case OptionType.BackToTitle:
                BackToTitle(option);
                break;
        }
    }

    // 5) �� �ɼ� ó�� �޼���
    private void ShowControlGuide(OptionType option)
    {
        // Control Guide ���� : ���� ����
        option = OptionType.ControlGuide;
        grid_ControlGuide.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void ToggleDisplayMode(OptionType option)
    {
        // Display Mode ����
        option = OptionType.DisplayMode;
        grid_DisplayMode.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void ChangeResolution(OptionType option)
    {
        // Resolution ���� ����
        option = OptionType.DisplayResolution;
        grid_DisplayResolution.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void OpenSoundSettings(OptionType option)
    {
        // Sound ���� ����
        option = OptionType.Sound;
        grid_SoundVolume.gameObject.SetActive(true);
        CompareOptionString(option);

        sl_EffectSound.onValueChanged.AddListener(OnValueChaged_SFX);
        sl_BackgroundSound.onValueChanged.AddListener(OnValueChaged_BGM);
        sl_BackgroundSound.onValueChanged.AddListener(OnValueChaged_UI);
    }

    public void OnValueChaged_SFX(float value)
    {
        float volume = value / (float)MAX_SOUND_VALUE;
    }

    //������� ����
    public void OnValueChaged_BGM(float value)
    {
        float volume = value / (float)MAX_SOUND_VALUE;
    }

    public void OnValueChaged_UI(float value)
    {
        float volume = value / (float)MAX_SOUND_VALUE;
    }

    private void OpenLanguageSettings(OptionType option)
    {
        // Language ȭ�� ����
        option = OptionType.Language;
        grid_ScrollView.gameObject.SetActive(true);
        CompareOptionString(option);

        //Language ���� �ٲٱ�

    }

    private void LoadLastCheckpoint(OptionType option)
    {
        option = OptionType.LoadCheckpoint;
        CompareOptionString(option);
        // üũ����Ʈ �ε� ���� : ���� ����
    }

    private void ShowCredits(OptionType option)
    {
        // Credit ȭ�� ����
        option = OptionType.Credit;
        grid_Credits.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void BackToTitle(OptionType option)
    {
        // Ÿ��Ʋ�� ���ư���
        option = OptionType.BackToTitle;
        CompareOptionString(option);
        if (Input.GetKey(KeyCode.Escape))
        {
            img_OptionsPage.gameObject.SetActive(false);
            print("Escape is Working");
        }
    }

    //Ŭ���ߴ� �ɼ�â�� ������ ���� ������ �ɼ�â�� ���̵���
    private void TurnOffShowedPage(GameObject optionPage, OptionType optionName)
    {
        for(int i = 0; i < optionButtons.Length; i++)
        {
           if(optionPage.ToString() != optionName.ToString())
           {
               img_Keys.SetActive(false); //Ű�е� �̹���
                print("optionPage.ToString : " + optionPage.ToString() + "//optionNAme.DisplayName : " + optionName.ToString());
           }
        }
    }

    private void CompareOptionString(OptionType optionName)
    {
        for (int i = 0; i < optionButtons.Length; ++i)
        {
            TurnOffShowedPage(optionButtons[i], optionName);
        }
    }
}