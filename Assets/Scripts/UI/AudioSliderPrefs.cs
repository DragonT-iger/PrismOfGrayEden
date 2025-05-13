using UnityEngine;
using UnityEngine.UI;

public class AudioSliderPrefs : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider bgmSlider;   // �ν����Ϳ� BGM �����̴� �Ҵ�
    [SerializeField] private Slider sfxSlider;   // �ν����Ϳ� SFX �����̴� �Ҵ�

    private const string BgmKey = "BackgroundMusicVolume";  // BGM ���� Ű
    private const string SfxKey = "EffectSoundVolume";     // SFX ���� Ű

    private void Awake()
    {
        // 1) ������ ����� �� �ҷ����� (�⺻ 1.0f)
        bgmSlider.value = PlayerPrefs.GetFloat(BgmKey, 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat(SfxKey, 1.0f);

        // 2) �� ���� �� �̺�Ʈ ���
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }

    // BGM �����̴� �� ���� �� ȣ��
    private void OnBgmSliderChanged(float newValue)
    {
        // TODO: AudioMixer �Ǵ� AudioManager�� ���� ���� ����
        // e.g. AudioManager.Instance.SetBgmVolume(newValue);

        PlayerPrefs.SetFloat(BgmKey, newValue);
        PlayerPrefs.Save();  // ��� ��ũ�� ���
    }

    // SFX �����̴� �� ���� �� ȣ��
    private void OnSfxSliderChanged(float newValue)
    {
        // TODO: AudioMixer �Ǵ� AudioManager�� ���� ���� ����
        // e.g. AudioManager.Instance.SetSfxVolume(newValue);

        PlayerPrefs.SetFloat(SfxKey, newValue);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // �޸� �� ������ ���� ������ ����
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChanged);
    }
}
