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

    // ���� ��� �ε��� (0: �Ϻ���, 1: �ѱ���, 2: ����, 3: �����ξ�)
    private static int currentLanguageIndex = 0;
    private TextMeshProUGUI textComponent;

    // ��� ���� �� ����� �̺�Ʈ ��������Ʈ
    public delegate void LanguageChangeHandler();
    public static event LanguageChangeHandler OnLanguageChanged;

    private void Awake() 
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    // ��� ���� �޼���
    public static void ChangeLanguage(int index) 
    {
        currentLanguageIndex = index;
        // ��� ���� �̺�Ʈ �߻�
        if (OnLanguageChanged != null)
        {
            OnLanguageChanged();
        }
            
    }

    // ���� ��� �ε��� ��������
    public static int GetCurrentLanguageIndex() 
    {
        return currentLanguageIndex;
    }

    private void OnEnable() 
    {
        // ��� ���� �̺�Ʈ ����
        OnLanguageChanged += Refresh;

        // �ʱ� �ؽ�Ʈ ����
        Refresh();
    }

    private void OnDisable() 
    {
        // ���� ���� (�޸� ���� ����)
        OnLanguageChanged -= Refresh;
    }

    public void Refresh() 
    {
        if (textComponent != null && languageData != null && !string.IsNullOrEmpty(textKey))
        {
            // ���� ��� �ε����� ���� ������ ��� ������ ����
            if (currentLanguageIndex < languageData.Sheet1.Count + 1)
            {
                MultiLanguage currentLanguage = languageData.Sheet1[currentLanguageIndex];
                
                var field = typeof(MultiLanguage).GetField(textKey);
                if (field != null)
                {
                    var value = field.GetValue(currentLanguage) as string;
                    textComponent.text = value;
                    //Debug.Log("�ε����� üũ��" + currentLanguageIndex);
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