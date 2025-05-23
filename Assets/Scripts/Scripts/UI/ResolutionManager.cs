using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    // 현재 풀스크린 여부
    private bool isFullScreen = false;

    private void Awake()
    {
        // 시작 시 무조건 창 모드로 설정
        SetWindowMode();
    }

    /// <summary>
    /// 풀스크린 모드로 전환
    /// </summary>
    public void SetFullScreenMode()
    {
        isFullScreen = true;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    /// <summary>
    /// 창 모드로 전환
    /// </summary>
    public void SetWindowMode()
    {
        isFullScreen = false;
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    /// <summary>
    /// 1920×1080 해상도로 변경
    /// </summary>
    public void SetFHD()
    {
        ApplyResolution(1920, 1080);
    }

    /// <summary>
    /// 2560×1440 해상도로 변경
    /// </summary>
    public void SetQHD()
    {
        ApplyResolution(2560, 1440);
    }

    // 공통 해상도 적용 로직
    private void ApplyResolution(int width, int height)
    {
        Screen.SetResolution(width, height, isFullScreen);
    }
}
