using UnityEngine;

public class DisplayMode : MonoBehaviour
{
    public bool isFullScreen = true;

    public void FullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        isFullScreen = true;
    }

    public void WindowMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        isFullScreen = false;
    }
}
