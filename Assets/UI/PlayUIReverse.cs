using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayUIReverse : MonoBehaviour
{
    [Tooltip("Canvas ���� �÷��� Full-Screen White Image")]
    [SerializeField] private Image fadeImage;

    [Tooltip("���̵� �ƿ� �ҿ� �ð�(��)")]
    [SerializeField] private float duration = 1f;

    private Color color;

    private void Awake()
    {
        if (fadeImage == null)
            Debug.LogError("Fade Image�� �Ҵ���� ����");

        // �ʱ� ���� 1 ����
        color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);
        StartFadeIn();
    }

    /// <summary>
    /// �ܺο��� ȣ���� �� ����� �� ������,
    /// ���� �ڷ�ƾ�� ���� ����ص� �����մϴ�.
    /// </summary>
    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    /// <summary>
    /// ���̵� �ƿ� �ڷ�ƾ�� public���� �����մϴ�.
    /// ȣ���� �ʿ��� yield return���� ��⵵ �����մϴ�.
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

        // Ȯ���� ���� ����������
        color.a = 0f;
        fadeImage.color = color;
    }
}