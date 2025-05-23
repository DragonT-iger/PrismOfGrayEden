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
    // 필요에 따라 계속 추가…
}

public class SavePointManager : MonoBehaviour
{
    [Header("버튼 프리팹")]
    [SerializeField] private GameObject saveButtonPrefab;

    [Header("버튼을 배치할 부모(예: GridLayoutGroup)")]
    [SerializeField] private Transform saveUIParent;

    [Header("씬 전환 애니메이터")]
    [SerializeField] private UIImageAnimator uiAnimator;

    private void Start()
    {
        if (saveButtonPrefab == null || saveUIParent == null || uiAnimator == null)
        {
            Debug.LogError("SavePointManager 설정 누락: Prefab, Parent, UIImageAnimator 중 할당 안 된 항목이 있음");
            return;
        }

        GenerateSaveButtons();
    }

    private void GenerateSaveButtons()
    {
        foreach (SavePoint sp in Enum.GetValues(typeof(SavePoint)))
        {
            GameObject btnObj = Instantiate(saveButtonPrefab, saveUIParent);

            // 1) Button 컴포넌트
            Button btn = btnObj.GetComponent<Button>();
            if (btn == null)
            {
                Debug.LogError($"{btnObj.name}에 Button 컴포넌트가 없습니다!");
                continue;
            }

            // 2) TextMeshProUGUI 컴포넌트
            TextMeshProUGUI label = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (label == null)
            {
                Debug.LogError($"{btnObj.name} 하위에 TextMeshProUGUI 컴포넌트가 없습니다!");
                continue;
            }

            // 3) 텍스트 설정 ("Stage" 접두사 제거)
            string raw = sp.ToString();          // "Stage1_1" 등
            string display = raw.StartsWith("Stage")
                ? raw.Substring("Stage".Length) // "1_1" 형태
                : raw;
            label.text = display;

            // 4) 클릭 리스너
            SavePoint targetPoint = sp;
            btn.onClick.AddListener(() =>
            {
                uiAnimator.PlayOneShotAndFade(targetPoint.ToString());
                SoundManager.Instance.PlaySFX("UIButtonClick");
            });
        }
    }
}