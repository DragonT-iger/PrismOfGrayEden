using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    // ���� Ǯ��ũ�� ����
    private bool isFullScreen = false;

    private void Awake()
    {
        // ���� �� ������ â ���� ����
        SetWindowMode();
    }

    /// <summary>
    /// Ǯ��ũ�� ���� ��ȯ
    /// </summary>
    public void SetFullScreenMode()
    {
        isFullScreen = true;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    /// <summary>
    /// â ���� ��ȯ
    /// </summary>
    public void SetWindowMode()
    {
        isFullScreen = false;
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    /// <summary>
    /// 1920��1080 �ػ󵵷� ����
    /// </summary>
    public void SetFHD()
    {
        ApplyResolution(1920, 1080);
    }

    /// <summary>
    /// 2560��1440 �ػ󵵷� ����
    /// </summary>
    public void SetQHD()
    {
        ApplyResolution(2560, 1440);
    }

    // ���� �ػ� ���� ����
    private void ApplyResolution(int width, int height)
    {
        Screen.SetResolution(width, height, isFullScreen);
    }
}
