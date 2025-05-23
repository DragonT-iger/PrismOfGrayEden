using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    // 파싱된 설정을 담을 프로퍼티
    public GameSettings CurrentSettings { get; set; }


    // 인스펙터에서 체크할 수 있는 디버그 모드 플래그
    [SerializeField] private bool debugMode = false;
    // 외부에서 읽기 전용으로 접근
    public bool DebugMode => debugMode;

    // 현재 활성화된 세이브 포인트
    private SavePoint currentSavePoint = SavePoint.Stage1_1;
    // 현재 세이브 포인트를 외부에서 읽기 전용으로 접근
    public SavePoint CurrentSavePoint => currentSavePoint;

    void Awake()
    {
        // 싱글톤 패턴 처리
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 화면 해상도 설정
        Screen.SetResolution(1920, 1080, false);
    }

    void Start()
    {
#if UNITY_EDITOR
        Debug.Log($"[GameManager] DebugMode = {debugMode}");
        Debug.Log($"[GameManager] Initial SavePoint = {currentSavePoint}");
#endif
    }

    void Update()
    {
        // 필요한 경우 디버그 모드에서 키 입력 등 처리 가능
    }

    /// <summary>
    /// 세이브 포인트를 변경합니다.
    /// </summary>
    public void SetSavePoint(SavePoint newPoint)
    {
        if (currentSavePoint == newPoint)
            return;

        currentSavePoint = newPoint;
#if UNITY_EDITOR
        if (debugMode)
            Debug.Log($"[GameManager] SavePoint 변경: {currentSavePoint}");
#endif
        // TODO: PlayerPrefs 등에 저장하거나, UI 갱신 처리 등을 추가
    }

    /// <summary>
    /// 현재 세이브 포인트를 반환합니다.
    /// </summary>
    public SavePoint GetSavePoint()
    {
        return currentSavePoint;
    }
}
