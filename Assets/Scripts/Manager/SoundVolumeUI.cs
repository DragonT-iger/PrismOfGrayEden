using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeUI : MonoBehaviour
{
    [Header("볼륨 슬라이더")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager가 씬에 없습니다!");
            enabled = false;
            return;
        }

        // 초기 슬라이더 값 세팅
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;

        // 이벤트 연결
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
    }

    private void OnBgmSliderChanged(float value)
    {
        SoundManager.Instance.SetBgmVolume(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }
}
