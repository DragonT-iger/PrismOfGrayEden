using TMPro;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] string textKey;
    [SerializeField] ExcelLanguage languageData;
    [SerializeField]  TMP_FontAsset[] fontArr;

    // 현재 언어 인덱스 (0: 일본어, 1: 한국어, 2: 영어, 3: 스페인어)
    private static int currentLanguageIndex = 0;
    private TextMeshProUGUI textComponent;

    // 언어 변경 시 사용할 이벤트 델리게이트
    public delegate void LanguageChangeHandler();
    public static event LanguageChangeHandler OnLanguageChanged;

    private void Awake() 
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    // 언어 변경 메서드
    public static void ChangeLanguage(int index) 
    {
        currentLanguageIndex = index;
        // 언어 변경 이벤트 발생
        if (OnLanguageChanged != null)
        {
            OnLanguageChanged();
        }
            
    }

    // 현재 언어 인덱스 가져오기
    public static int GetCurrentLanguageIndex() 
    {
        return currentLanguageIndex;
    }

    private void OnEnable() 
    {
        // 언어 변경 이벤트 구독
        OnLanguageChanged += Refresh;

        // 초기 텍스트 설정
        Refresh();
    }

    private void OnDisable() 
    {
        // 구독 해제 (메모리 누수 방지)
        OnLanguageChanged -= Refresh;
    }

    public void Refresh() 
    {
        if (textComponent != null && languageData != null && !string.IsNullOrEmpty(textKey))
        {
            // 현재 언어 인덱스에 따라 적절한 언어 데이터 선택
            if (currentLanguageIndex < languageData.Sheet1.Count + 1)
            {
                MultiLanguage currentLanguage = languageData.Sheet1[currentLanguageIndex];
                
                var field = typeof(MultiLanguage).GetField(textKey);
                if (field != null)
                {
                    var value = field.GetValue(currentLanguage) as string;
                    textComponent.text = value;
                    //Debug.Log("인덱스값 체크용" + currentLanguageIndex);
                    textComponent.font = fontArr[currentLanguageIndex];
                    
                }
                /*else
                {
                    //Debug.LogWarning($"Field '{textKey}' not found in MultiLanguage");
                }
            else
            {
                Debug.LogWarning($"Language index {currentLanguageIndex} is out of range");
            }*/
            }
        }
    }
}