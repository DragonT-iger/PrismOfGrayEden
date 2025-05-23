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
            Debug.LogError("[SpriteCutScene] cutSceneImage 필드가 비어있습니다!");

        HideCutScene();
    }

    public void ShowCutScene(int index)
    {
        if (cutSceneImage == null)
            return;

        if (index < 0 || index >= cutSceneSprites.Length)
        {
            Debug.LogWarning($"[SpriteCutScene] 유효하지 않은 인덱스 {index}");
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
