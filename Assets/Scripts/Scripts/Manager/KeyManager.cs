using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }

    // keyID ���ڿ� �� ȹ�� ����
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

        // PlayerPrefs �κ��� �ҷ�����
        LoadAll();
    }

    // key ȹ�� ó��
    public void Acquire(string keyID) {
        acquired[keyID] = true;
        PlayerPrefs.SetInt(PREF_PREFIX + keyID, 1);
        PlayerPrefs.Save();
    }

    // key ȹ�� ���� ��ȸ
    public bool IsAcquired(string keyID) {
        bool val;
        if (acquired.TryGetValue(keyID, out val))
            return val;
        return false;
    }

    // ��� Ű �ʱ�ȭ (�׽�Ʈ/����׿�)
    public void ResetAll() {
        var keys = new List<string>(acquired.Keys);
        foreach (var id in keys)
        {
            PlayerPrefs.DeleteKey(PREF_PREFIX + id);
            acquired[id] = false;
        }
        PlayerPrefs.Save();
        Debug.Log("[KeyManager] ��� Ű ���µ�");
    }

    // ����� Ű ���� �ҷ�����
    private void LoadAll() {
        // (PlayerPrefs �� � keyID�� ����ƴ��� �̸� �� �� �����Ƿ�,
        //  �������� �̸� ���ǵ� keyID ����� �ϳ� �ΰ� foreach ������ �մϴ�.)
        // ���÷�, enum �̳� �ؽ�Ʈ ���� ������ �����ϼ���.
    }
}
