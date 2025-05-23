using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerTest : MonoBehaviour
{
    public static SoundManagerTest Instance;

    [Header("Mixer")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Mixer Groups")]
    [SerializeField] AudioMixerGroup bgmGroup;
    [SerializeField] AudioMixerGroup sfxGroup;

    [Header("Audio Sources")]
    AudioSource bgmSource;
    List<AudioSource> sfxSources = new List<AudioSource>();

    [Header("Clips")]
    [SerializeField] AudioClip[] bgmClips; 
    [SerializeField] AudioClip[] sfxClips; 

    [SerializeField] static float bgmVolume = 1f;
    [SerializeField] static float sfxVolume = 1f;

    private void Awake() 
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitAudioSources();
            SetVolumes(); // �ʱ� ���� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitAudioSources() 
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmSource.loop = true;

        // SFX�� ��ø �����ϹǷ� ���� AudioSource �غ�
        for (int i = 0; i < 10; i++)
        {
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfx.outputAudioMixerGroup = sfxGroup;
            sfxSources.Add(sfx);
        }
    }

    private void SetVolumes() 
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(bgmVolume) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }

    // BGM
    public void PlayBGM(int index) 
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }
    }

    public void StopBGM() 
    {
        bgmSource.Stop();
    }

    // SFX
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            foreach (AudioSource source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.clip = sfxClips[index];
                    source.Play();
                    return;
                }
            }
            // ���� ���� ��� ���̸�, ������ ù ��° ����
            sfxSources[0].Stop();
            sfxSources[0].clip = sfxClips[index];
            sfxSources[0].Play();
        }
    }

    // �����̴����� ���� �� ȣ��
    public void SetBGMVolume(float value) 
    {
        bgmVolume = value;
        
        if (value <= 0.0001f)
            audioMixer.SetFloat("BGMVolume", -80f);
        else
            audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20f);
    }

    public void SetSFXVolume(float value) 
    {
        sfxVolume = value;

        if (value <= 0.0001f)
            audioMixer.SetFloat("SFXVolume", -80f);
        else
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }
}
