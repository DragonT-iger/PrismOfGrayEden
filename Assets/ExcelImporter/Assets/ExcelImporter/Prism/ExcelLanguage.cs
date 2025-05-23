using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ExcelLanguage : ScriptableObject
{
    public List<MultiLanguage> Sheet1 = new List<MultiLanguage>();
}
[System.Serializable]
public class MultiLanguage
{
    public string languageName;
    public string TitleName;
    public string StartMessage;
    public string StartButtonText;
    public string ContinueButtonText;
    public string OptionsButtonText;
    public string QuitButtonText;
    public string ControlGuideText;
    public string MoveGuideText;
    public string DashGuideText;
    public string HoldGuideText;
    public string EmergencyEvadeGuideText;
    public string KeyboardGuideText;
    public string GamePadGuideText;
    public string CheckpointLoadMenuText;
    public string CheckpointLoadCheckText;
    public string LoadConfirmText;
    public string LoadCancelText;
    public string DisplayModeMenuText;
    public string WindowedModeText;
    public string FullScreenModeText;
    public string DisplayResolutionMenuText;
    public string SoundMenuText;
    public string BGMSliderText;
    public string SFXSliderText;
    public string LanguageListText;
    public string CreditMenuText;
    public string FontName;
    public string MenuText;
}