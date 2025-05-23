using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    // keyID 문자열 → 획득 여부
    private Dictionary<string, bool> acquired = new Dictionary<string, bool>();

    private const string PREF_PREFIX = "KeyAq_";

    void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // PlayerPrefs 로부터 불러오기
        LoadAll();
    }

    // key 획득 처리
    public void Acquire(string keyID) {
        acquired[keyID] = true;
        PlayerPrefs.SetInt(PREF_PREFIX + keyID, 1);
        PlayerPrefs.Save();
    }

    // key 획득 여부 조회
    public bool IsAcquired(string keyID) {
        bool val;
        if (acquired.TryGetValue(keyID, out val))
            return val;
        return false;
    }

    // 모든 키 초기화 (테스트/디버그용)
    public void ResetAll() {
        var keys = new List<string>(acquired.Keys);
        foreach (var id in keys)
        {
            PlayerPrefs.DeleteKey(PREF_PREFIX + id);
            acquired[id] = false;
        }
        PlayerPrefs.Save();
        Debug.Log("[KeyManager] 모든 키 리셋됨");
    }

    // 저장된 키 전부 불러오기
    private void LoadAll() {
        // (PlayerPrefs 에 어떤 keyID가 저장됐는지 미리 알 수 없으므로,
        //  실전에선 미리 정의된 keyID 목록을 하나 두고 foreach 돌려야 합니다.)
        // 예시로, enum 이나 텍스트 파일 등으로 관리하세요.
    }
}
