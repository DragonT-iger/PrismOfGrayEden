using UnityEngine;

public class LanguageDebugger : MonoBehaviour
{
    private const string PrefKey = "LanguageIndex";

    [SerializeField]
    private int number = 0;

    private void Awake()
    {
        // ����� �� �ҷ����� (�⺻�� 0)
        number = PlayerPrefs.GetInt(PrefKey, 0);
        // �ҷ��� ������ ��� ����
        LocalizedText.ChangeLanguage(number);
    }

    /// <summary>
    /// Inspector���� ȣ��: ��� �ε����� �Է¹޾� ���� �� ����
    /// </summary>
    public void ToggleAutoSwitch(int input)
    {
        number = input;
        // PlayerPrefs�� ����
        PlayerPrefs.SetInt(PrefKey, number);
        PlayerPrefs.Save();
        // ���� ��� ���� ȣ��
        LocalizedText.ChangeLanguage(number);
    }
}
