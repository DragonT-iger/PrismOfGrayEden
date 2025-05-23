using UnityEngine;

public class OptionToggle : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;  // 에디터에서 할당
    private float previousTimeScale = 1f;          // 복원할 원래 timescale

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionUI == null)
            {
                Debug.LogWarning("OptionToggle: optionUI가 할당되지 않았습니다.");
                return;
            }

            // UI 활성 상태 토글
            bool willShow = !optionUI.activeSelf;
            optionUI.SetActive(willShow);

            // timescale 설정
            if (willShow)
            {
                // 일시정지 직전의 timescale 저장 후 0으로
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                // 이전 timescale 복원
                Time.timeScale = 1f;
            }
        }
    }

    public void SetTimeScaleOne()
    {
        Time.timeScale = 1f;
    }
}
