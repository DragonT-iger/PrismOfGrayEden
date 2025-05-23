using UnityEngine;
using UnityEngine.UI;

public class AudioSliderPrefs : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider bgmSlider;   // 인스펙터에 BGM 슬라이더 할당
    [SerializeField] private Slider sfxSlider;   // 인스펙터에 SFX 슬라이더 할당

    private const string BgmKey = "BackgroundMusicVolume";  // BGM 저장 키
    private const string SfxKey = "EffectSoundVolume";     // SFX 저장 키

    private void Awake()
    {
        // 1) 이전에 저장된 값 불러오기 (기본 1.0f)
        bgmSlider.value = PlayerPrefs.GetFloat(BgmKey, 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat(SfxKey, 1.0f);

        // 2) 값 변경 시 이벤트 등록
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }

    // BGM 슬라이더 값 변경 시 호출
    private void OnBgmSliderChanged(float newValue)
    {
        // TODO: AudioMixer 또는 AudioManager에 실제 볼륨 적용
        // e.g. AudioManager.Instance.SetBgmVolume(newValue);

        PlayerPrefs.SetFloat(BgmKey, newValue);
        PlayerPrefs.Save();  // 즉시 디스크에 기록
    }

    // SFX 슬라이더 값 변경 시 호출
    private void OnSfxSliderChanged(float newValue)
    {
        // TODO: AudioMixer 또는 AudioManager에 실제 볼륨 적용
        // e.g. AudioManager.Instance.SetSfxVolume(newValue);

        PlayerPrefs.SetFloat(SfxKey, newValue);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // 메모리 릭 방지를 위해 리스너 해제
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
    }
}
