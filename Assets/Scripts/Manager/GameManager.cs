using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static GameManager Instance { get; private set; }

    // �Ľ̵� ������ ���� ������Ƽ
    public GameSettings CurrentSettings { get; set; }


    // �ν����Ϳ��� üũ�� �� �ִ� ����� ��� �÷���
    [SerializeField] private bool debugMode = false;
    // �ܺο��� �б� �������� ����
    public bool DebugMode => debugMode;

    // ���� Ȱ��ȭ�� ���̺� ����Ʈ
    private SavePoint currentSavePoint = SavePoint.Stage1_1;
    // ���� ���̺� ����Ʈ�� �ܺο��� �б� �������� ����
    public SavePoint CurrentSavePoint => currentSavePoint;

    void Awake()
    {
        // �̱��� ���� ó��
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ȭ�� �ػ� ����
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
        // �ʿ��� ��� ����� ��忡�� Ű �Է� �� ó�� ����
    }

    /// <summary>
    /// ���̺� ����Ʈ�� �����մϴ�.
    /// </summary>
    public void SetSavePoint(SavePoint newPoint)
    {
        if (currentSavePoint == newPoint)
            return;

        currentSavePoint = newPoint;
#if UNITY_EDITOR
        if (debugMode)
            Debug.Log($"[GameManager] SavePoint ����: {currentSavePoint}");
#endif
        // TODO: PlayerPrefs � �����ϰų�, UI ���� ó�� ���� �߰�
    }

    /// <summary>
    /// ���� ���̺� ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    public SavePoint GetSavePoint()
    {
        return currentSavePoint;
    }
}
