using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("���� ���")]
    [SerializeField] private Sound[] bgmSounds;
    [SerializeField] private Sound[] sfxSounds;

    // ���� ���� ������ҽ� ��������������������������
    private AudioSource bgmSource;
    private AudioSource sfxSource;          // �Ͻ����� ������ ����
    private AudioSource unpausableSfxSource; // �Ͻ����� ����

    // ���� ���� ���� ��������������������������������
    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> sfxDict;

    // ���� ���� �� ��������������������������������������
    private float bgmMasterVolume = 1f;
    private float sfxMasterVolume = 1f;
    private string currentBgmName;

    #region �������
    private void Awake()
    {
        // �̱���
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitAudioSources();
        BuildDictionaries();
        LoadSavedVolumes();
        ApplyVolume();          // ���尪 ��� ����
    }
    #endregion

    #region �ʱ�ȭ ���� �޼���
    private void InitAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.spatialBlend = 0f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.spatialBlend = 0f;

        unpausableSfxSource = gameObject.AddComponent<AudioSource>();
        unpausableSfxSource.spatialBlend = 0f;
        unpausableSfxSource.ignoreListenerPause = true;
    }

    private void BuildDictionaries()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var s in bgmSounds)
            if (!string.IsNullOrEmpty(s.name) && s.clip && !bgmDict.ContainsKey(s.name))
                bgmDict.Add(s.name, s.clip);

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var s in sfxSounds)
            if (!string.IsNullOrEmpty(s.name) && s.clip && !sfxDict.ContainsKey(s.name))
                sfxDict.Add(s.name, s.clip);
    }

    private void LoadSavedVolumes()
    {
        bgmMasterVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxMasterVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void ApplyVolume()
    {
        bgmSource.volume = bgmMasterVolume;
        sfxSource.volume = sfxMasterVolume;
        unpausableSfxSource.volume = sfxMasterVolume;
    }
    #endregion

    #region �ܺο� ������ ���� ������Ƽ/�޼���
    public float BgmVolume
    {
        get => bgmMasterVolume;
        set => SetBgmVolume(value);
    }

    public float SfxVolume
    {
        get => sfxMasterVolume;
        set => SetSfxVolume(value);
    }

    public void SetBgmVolume(float value)
    {
        bgmMasterVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("BGMVolume", bgmMasterVolume);
        PlayerPrefs.Save();
        bgmSource.volume = bgmMasterVolume;
    }

    public void SetSfxVolume(float value)
    {
        sfxMasterVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("SFXVolume", sfxMasterVolume);
        PlayerPrefs.Save();
        sfxSource.volume = sfxMasterVolume;
        unpausableSfxSource.volume = sfxMasterVolume;
    }
    #endregion

    #region BGM ����
    public void PlayBGM(string soundName)
    {
        if (bgmSource.isPlaying && currentBgmName == soundName) return;

        if (bgmDict.TryGetValue(soundName, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
            currentBgmName = soundName;
        }
        else
        {
            Debug.LogWarning($"[SoundManager] BGM not found: {soundName}");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        currentBgmName = null;
    }
    #endregion

    #region SFX ����
    public void PlaySFX(string soundName)
    {
        if (sfxDict.TryGetValue(soundName, out var clip))
            sfxSource.PlayOneShot(clip, sfxMasterVolume);
        else
            Debug.LogWarning($"[SoundManager] SFX not found: {soundName}");
    }

    public void PlayUnpausableSFX(string soundName)
    {
        if (sfxDict.TryGetValue(soundName, out var clip))
            unpausableSfxSource.PlayOneShot(clip, sfxMasterVolume);
        else
            Debug.LogWarning($"[SoundManager] Unpausable SFX not found: {soundName}");
    }

    public void PauseSFX(bool isPaused)
    {
        if (isPaused) sfxSource.Pause();
        else sfxSource.UnPause();
    }
    #endregion
}
