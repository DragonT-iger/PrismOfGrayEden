using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GameSettings
{
    public GameSettings CurrentSettings { get; set; }
    public string playerName;
    public int highScore;
    public float volume;
}

public class DataLoader : MonoBehaviour
{
    // StreamingAssets/MyFolder/data.json 을 읽어올 예시
    private void Start()
    {
        StartCoroutine(LoadJsonCoroutine());
    }

    private IEnumerator LoadJsonCoroutine()
    {
        // 플랫폼별 경로 얻기
        string basePath = Application.streamingAssetsPath;
        string fullPath = Path.Combine(basePath, "MyFolder", "data.json");

        UnityWebRequest req = UnityWebRequest.Get(fullPath);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"파일 로드 실패: {req.error}");
            yield break;
        }

        string json = req.downloadHandler.text;
        Debug.Log("읽어온 JSON:\n" + json);

        GameSettings settings = JsonUtility.FromJson<GameSettings>(json);
        GameManager.Instance.CurrentSettings = settings;

}
}
