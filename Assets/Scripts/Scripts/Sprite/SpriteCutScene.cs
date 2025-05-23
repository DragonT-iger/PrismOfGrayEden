// SpriteCutScene.cs
using UnityEngine;
using UnityEngine.UI;

public class SpriteCutScene : MonoBehaviour
{
    [SerializeField] private Image cutSceneImage;
    [SerializeField] private Sprite[] cutSceneSprites;

    public static SpriteCutScene Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (cutSceneImage == null)
            Debug.LogError("[SpriteCutScene] cutSceneImage �ʵ尡 ����ֽ��ϴ�!");

        HideCutScene();
    }

    public void ShowCutScene(int index)
    {
        if (cutSceneImage == null)
            return;

        if (index < 0 || index >= cutSceneSprites.Length)
        {
            Debug.LogWarning($"[SpriteCutScene] ��ȿ���� ���� �ε��� {index}");
            return;
        }

        cutSceneImage.sprite = cutSceneSprites[index];
        cutSceneImage.gameObject.SetActive(true);
    }

    public void HideCutScene()
    {
        if (cutSceneImage == null)
            return;

        cutSceneImage.gameObject.SetActive(false);
    }
}
