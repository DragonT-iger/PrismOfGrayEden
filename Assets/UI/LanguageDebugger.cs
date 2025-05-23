using UnityEngine;

public class LanguageDebugger : MonoBehaviour
{
    private const string PrefKey = "LanguageIndex";

    [SerializeField]
    private int number = 0;

    private void Awake()
    {
        // 저장된 값 불러오기 (기본값 0)
        number = PlayerPrefs.GetInt(PrefKey, 0);
        // 불러온 값으로 언어 변경
        LocalizedText.ChangeLanguage(number);
    }

    /// <summary>
    /// Inspector에서 호출: 언어 인덱스를 입력받아 변경 및 저장
    /// </summary>
    public void ToggleAutoSwitch(int input)
    {
        number = input;
        // PlayerPrefs에 저장
        PlayerPrefs.SetInt(PrefKey, number);
        PlayerPrefs.Save();
        // 실제 언어 변경 호출
        LocalizedText.ChangeLanguage(number);
    }
}
