using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Options Page

// 1) 옵션 종류를 enum으로 정의
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
    public GameObject txt_optionTitle;//settings 클릭 후 발생하는 option창의 타이틀 텍스트
    public GameObject img_OptionsPage;
    public GameObject grid_ControlGuide;
    public GameObject img_Keys;       //키패드 설정 이미지
    public GameObject grid_DisplayMode;
    public GameObject grid_DisplayResolution;
    public GameObject grid_SoundVolume;
    public GameObject grid_ScrollView;
    public GameObject grid_Credits;

    int MAX_SOUND_VALUE = 1;

    // 2) Inspector에서 순서대로 버튼을 할당 (enum 순서와 동일하게)
    [SerializeField] private GameObject[] optionButtons;

    [SerializeField] private Grid[] option;

    [SerializeField] private GameObject txt_options;

    private void Awake()
    {
        // enum 개수와 배열 길이 맞추기 체크
        var enumCount = System.Enum.GetNames(typeof(OptionType)).Length;
        if (optionButtons.Length != enumCount)
        {
            Debug.Log($"OptionSettingsCtrl: 버튼 배열 길이({optionButtons.Length})가 OptionType 개수({enumCount})와 다릅니다.");
        }

        //for(int i = 0; i < optionButtons.Length; i++)
        //{
        //    optionButtons[i].SetActive(true); 
        //}
    }

    public void Start()
    {
        img_OptionsPage.gameObject.SetActive(false); //전체 하위 오브젝트까지 모두 disabled
        txt_optionTitle.gameObject.SetActive(true); //옵션 창의 타이틀 텍스트 options 활성화
        optionButtons[(int)OptionType.ControlGuide].SetActive(true); //옵션창 뜨면 기본적으로 ctrl guide만 뜨도록
    }

    // 4) 클릭 시 호출되는 공통 메서드
    public void OnClickOption(OptionType option)
    {
        // 예: 클릭된 옵션 이름을 텍스트에 표시
        txt_options.gameObject.name = $"선택된 옵션: {option}";
        
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

    // 5) 각 옵션 처리 메서드
    private void ShowControlGuide(OptionType option)
    {
        // Control Guide 로직 : 추후 수정
        option = OptionType.ControlGuide;
        grid_ControlGuide.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void ToggleDisplayMode(OptionType option)
    {
        // Display Mode 로직
        option = OptionType.DisplayMode;
        grid_DisplayMode.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void ChangeResolution(OptionType option)
    {
        // Resolution 변경 로직
        option = OptionType.DisplayResolution;
        grid_DisplayResolution.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void OpenSoundSettings(OptionType option)
    {
        // Sound 설정 로직
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

    //배경음악 음량
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
        // Language 화면 띄우기
        option = OptionType.Language;
        grid_ScrollView.gameObject.SetActive(true);
        CompareOptionString(option);

        //Language 설정 바꾸기

    }

    private void LoadLastCheckpoint(OptionType option)
    {
        option = OptionType.LoadCheckpoint;
        CompareOptionString(option);
        // 체크포인트 로드 로직 : 추후 구현
    }

    private void ShowCredits(OptionType option)
    {
        // Credit 화면 띄우기
        option = OptionType.Credit;
        grid_Credits.gameObject.SetActive(true);
        CompareOptionString(option);
    }

    private void BackToTitle(OptionType option)
    {
        // 타이틀로 돌아가기
        option = OptionType.BackToTitle;
        CompareOptionString(option);
        if (Input.GetKey(KeyCode.Escape))
        {
            img_OptionsPage.gameObject.SetActive(false);
            print("Escape is Working");
        }
    }

    //클릭했던 옵션창을 지워서 새로 선택한 옵션창만 보이도록
    private void TurnOffShowedPage(GameObject optionPage, OptionType optionName)
    {
        for(int i = 0; i < optionButtons.Length; i++)
        {
           if(optionPage.ToString() != optionName.ToString())
           {
               img_Keys.SetActive(false); //키패드 이미지
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