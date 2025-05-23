using UnityEngine;

public class UIButtonSoundManager : MonoBehaviour
{
    private string click = "UIButtonClick";
    private string hover = "UIButtonHover";
    public void SetClickSFX() { SoundManager.Instance.PlaySFX(click); }
    public void SetHoverSFX() { SoundManager.Instance.PlaySFX(hover); }

    

}
