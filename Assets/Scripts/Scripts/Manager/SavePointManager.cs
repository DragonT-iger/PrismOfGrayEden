// SavePointManager.cs
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SavePoint
{ 
    Stage1_1,
    Stage1_2,
    Stage1_3,
    Stage1_4,
    Stage2_1
    // �ʿ信 ���� ��� �߰���
}

public class SavePointManager : MonoBehaviour
{
    [Header("��ư ������")]
    [SerializeField] private GameObject saveButtonPrefab;

    [Header("��ư�� ��ġ�� �θ�(��: GridLayoutGroup)")]
    [SerializeField] private Transform saveUIParent;

    [Header("�� ��ȯ �ִϸ�����")]
    [SerializeField] private UIImageAnimator uiAnimator;

    private void Start()
    {
        if (saveButtonPrefab == null || saveUIParent == null || uiAnimator == null)
        {
            Debug.LogError("SavePointManager ���� ����: Prefab, Parent, UIImageAnimator �� �Ҵ� �� �� �׸��� ����");
            return;
        }

        GenerateSaveButtons();
    }

    private void GenerateSaveButtons()
    {
        foreach (SavePoint sp in Enum.GetValues(typeof(SavePoint)))
        {
            GameObject btnObj = Instantiate(saveButtonPrefab, saveUIParent);

            // 1) Button ������Ʈ
            Button btn = btnObj.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogError($"{btnObj.name}�� Button ������Ʈ�� �����ϴ�!");
                continue;
            }

            // 2) TextMeshProUGUI ������Ʈ
            TextMeshProUGUI label = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (label == null)
            {
                Debug.LogError($"{btnObj.name} ������ TextMeshProUGUI ������Ʈ�� �����ϴ�!");
                continue;
            }

            // 3) �ؽ�Ʈ ���� ("Stage" ���λ� ����)
            string raw = sp.ToString();          // "Stage1_1" ��
            string display = raw.StartsWith("Stage")
                ? raw.Substring("Stage".Length) // "1_1" ����
                : raw;
            label.text = display;

            // 4) Ŭ�� ������
            SavePoint targetPoint = sp;
            btn.onClick.AddListener(() =>
            {
                uiAnimator.PlayOneShotAndFade(targetPoint.ToString());
                SoundManager.Instance.PlaySFX("UIButtonClick");
            });
        }
    }
}