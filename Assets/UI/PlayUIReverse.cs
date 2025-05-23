using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayUIReverse : MonoBehaviour
{
    [Tooltip("Canvas 위에 올려둔 Full-Screen White Image")]
    [SerializeField] private Image fadeImage;

    [Tooltip("페이드 아웃 소요 시간(초)")]
    [SerializeField] private float duration = 1f;

    private Color color;

    private void Awake()
    {
        if (fadeImage == null)
            Debug.LogError("Fade Image가 할당되지 않음");

        // 초기 알파 1 세팅
        color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);
        StartFadeIn();
    }

    /// <summary>
    /// 외부에서 호출할 때 사용할 수 있지만,
    /// 내부 코루틴을 직접 사용해도 무방합니다.
    /// </summary>
    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    /// <summary>
    /// 페이드 아웃 코루틴을 public으로 노출합니다.
    /// 호출한 쪽에서 yield return으로 대기도 가능합니다.
    /// </summary>
    public IEnumerator FadeOutCoroutine()
    {
        fadeImage.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            color.a = (duration - Mathf.Clamp01(elapsed / duration)) / duration;
            fadeImage.color = color;
            yield return null;
        }

        // 확실히 완전 불투명으로
        color.a = 0f;
        fadeImage.color = color;
    }
}