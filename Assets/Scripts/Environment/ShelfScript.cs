// ShelfScript.cs
using UnityEngine;

public class ShelfScript : MonoBehaviour
{
    [SerializeField] private int index = 0;
    public bool isKeyAquired = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isKeyAquired || !collision.gameObject.CompareTag("Player"))
            return;

        if (SpriteCutScene.Instance != null)
            SpriteCutScene.Instance.ShowCutScene(index);
        else
            Debug.LogError("[ShelfScript] SpriteCutScene.Instance가 null입니다!");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (SpriteCutScene.Instance != null)
            SpriteCutScene.Instance.HideCutScene();
        else
            Debug.LogError("[ShelfScript] SpriteCutScene.Instance가 null입니다!");
    }
}
