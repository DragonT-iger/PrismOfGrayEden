using UnityEngine;

public class OptionToggle : MonoBehaviour
{
    [SerializeField] private GameObject optionUI;  // �����Ϳ��� �Ҵ�
    private float previousTimeScale = 1f;          // ������ ���� timescale

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionUI == null)
            {
                Debug.LogWarning("OptionToggle: optionUI�� �Ҵ���� �ʾҽ��ϴ�.");
                return;
            }

            // UI Ȱ�� ���� ���
            bool willShow = !optionUI.activeSelf;
            optionUI.SetActive(willShow);

            // timescale ����
            if (willShow)
            {
                // �Ͻ����� ������ timescale ���� �� 0����
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                // ���� timescale ����
                Time.timeScale = 1f;
            }
        }
    }

    public void SetTimeScaleOne()
    {
        Time.timeScale = 1f;
    }
}
