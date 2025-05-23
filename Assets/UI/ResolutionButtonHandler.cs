using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionButtonHandler : MonoBehaviour
{
    List<Resolution> resolutions = new List<Resolution>();

    static readonly int width = 1920;
    static readonly int height = 1080;

    bool fullscreen = true;
    

    void Awake() 
    {
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 1920, height = 1200 });
        resolutions.Add(new Resolution { width = 2048, height = 1280 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 2560, height = 1600 });
        resolutions.Add(new Resolution { width = 2880, height = 1800 });
        resolutions.Add(new Resolution { width = 3480, height = 2160 });
    }

    public void SetResolution(int resolutionIndex) 
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log($"Resolution changed to: {resolution.width}x{resolution.height}, fullscreen: {fullscreen}");
    }
    public void OnResolutionButtonClicked() 
    {
        Screen.SetResolution(resolutions[0].width, resolutions[0].height, fullscreen);

        Debug.Log($"Resolution changed to: {resolutions[0].width}x{resolutions[0].height}, fullscreen: {fullscreen}");
    }

    public void OnResolutionButtonClicked1() 
    {
        Screen.SetResolution(resolutions[1].width, resolutions[1].height, fullscreen);
        Debug.Log($"Resolution changed to: {resolutions[1].width}x{resolutions[1].height}, fullscreen: {fullscreen}");
    }

    public void OnResolutionButtonClicked2() 
    {
        Screen.SetResolution(resolutions[2].width, resolutions[2].height, fullscreen);
        Debug.Log($"Resolution changed to: {resolutions[2].width}x{resolutions[2].height}, fullscreen: {fullscreen}");
    }
    public void OnResolutionButtonClicked3() 
    {
        Screen.SetResolution(resolutions[3].width, resolutions[3].height, fullscreen);
        Debug.Log($"Resolution changed to: {resolutions[3].width}x{resolutions[3].height}, fullscreen: {fullscreen}");
    }

}
