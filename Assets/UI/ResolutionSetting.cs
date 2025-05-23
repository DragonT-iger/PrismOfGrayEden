using UnityEngine;
public class ResolutionSetting : MonoBehaviour
{
    public DisplayMode dis;
    public void SetFHD() { Screen.SetResolution(1920, 1080, dis.isFullScreen); }
    public void SetQHD() { Screen.SetResolution(2560, 1440, dis.isFullScreen); }
}