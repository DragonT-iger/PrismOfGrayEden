using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeUI : MonoBehaviour
{
    [Header("���� �����̴�")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager�� ���� �����ϴ�!");
            enabled = false;
            return;
        }

        // �ʱ� �����̴� �� ����
        bgmSlider.value = SoundManager.Instance.BgmVolume;
        sfxSlider.value = SoundManager.Instance.SfxVolume;

        // �̺�Ʈ ����
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }

    private void OnDestroy()
    {
        // �޸� ���� ����
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
