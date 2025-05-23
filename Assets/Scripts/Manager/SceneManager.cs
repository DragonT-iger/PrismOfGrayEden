using System;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CheckAndPlayBGM(USceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        USceneManager.LoadScene(sceneName);
        CheckAndPlayBGM(sceneName);
        switch (sceneName)
        {
            case "Stage1_1":
                GameManager.Instance.SetSavePoint(SavePoint.Stage1_1);
                break;
            case "Stage1_2":
                GameManager.Instance.SetSavePoint(SavePoint.Stage1_2);
                break;
            case "Stage1_3":
                GameManager.Instance.SetSavePoint(SavePoint.Stage1_3);
                break;
            case "Stage1_4":
                GameManager.Instance.SetSavePoint(SavePoint.Stage1_4);
                break;
            default:
                break;
        }
        Time.timeScale = 1.0f;
    }


    public void ReloadScene()
    {
        var current = USceneManager.GetActiveScene().name;
        USceneManager.LoadScene(current);
    }

    public void LoadNextScene()
    {
        int nextIndex = USceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < USceneManager.sceneCountInBuildSettings)
        {
            var nextName = USceneManager.GetSceneByBuildIndex(nextIndex).name;
            USceneManager.LoadScene(nextIndex);
        }
        else
        {
            LoadScene("Stage1");
        }
    }

    // sceneName에 따라 알맞은 BGM 호출
    void CheckAndPlayBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                SoundManager.Instance.PlayBGM("MainMenu");
                break;
            case "Stage1_1":
            case "Stage1_2":
            case "Stage1_3":
            case "Stage1_4":
                SoundManager.Instance.PlayBGM("Stage1");
                break;
            case "Stage2":
                SoundManager.Instance.PlayBGM("Stage2");
                break;
            // 필요한 씬 이름과 BGM 키 추가…
            default:
                SoundManager.Instance.StopBGM();
                break;
        }
    }

    public void LoadSavePointScene()
    {
        LoadScene(GameManager.Instance.GetSavePoint().ToString());
        Debug.Log("asdasd");
    }
}
