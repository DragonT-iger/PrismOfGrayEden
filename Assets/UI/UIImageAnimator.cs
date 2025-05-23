using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIImageAnimator : MonoBehaviour
{
    [Header("�ִϸ��̼� ��� Image")]
    [SerializeField] private Image targetImage;

    [Header("�⺻ �ݺ� �ִϸ��̼�")]
    [SerializeField] private Sprite[] loopSprites;

    [Header("�� ���� ����� �ִϸ��̼�")]
    [SerializeField] private Sprite[] oneShotSprites;

    [Header("������ �� ��� �ð�(��)")]
    [SerializeField] private float frameDuration = 0.2f;

    [Header("���̵� �ƿ� ��Ʈ�ѷ�")]
    [SerializeField] private PlayUI fadeController;

    private Coroutine currentCoroutine;
    public bool hasPlayedOneShot = false;  // �� �� ���� ��� �÷���

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (fadeController == null)
            Debug.LogError("PlayUI(Fade ��Ʈ�ѷ�)�� �Ҵ���� ����");
        hasPlayedOneShot = false;
    }

    private void OnEnable()
    {
        hasPlayedOneShot = false;
        PlayLoopAnimation();
    }

    private void OnDisable()
    {
        StopCurrent();
    }

    public void PlayLoopAnimation()
    {
        StopCurrent();
        currentCoroutine = StartCoroutine(PlayLoop(loopSprites));
    }

    /// <summary>
    /// one-shot �ִϸ��̼� ��� �� ���̵� & �� ��ȯ
    /// ���ϴ� �� �̸��� �Ķ���ͷ� ����
    /// </summary>
    public void PlayOneShotAndFade(string sceneName)
    {

        
        if (hasPlayedOneShot) return;
        hasPlayedOneShot = true;

        StopCurrent();
        currentCoroutine = StartCoroutine(PlayOnceAndFade(oneShotSprites, sceneName));


    }

    private IEnumerator PlayLoop(Sprite[] sprites)
    {
        if (sprites == null || sprites.Length == 0)
            yield break;

        int idx = 0;
        while (true)
        {
            targetImage.sprite = sprites[idx];
            idx = (idx + 1) % sprites.Length;
            yield return new WaitForSecondsRealtime(frameDuration);
        }
    }

    private IEnumerator PlayOnceAndFade(Sprite[] sprites, string sceneName)
    {
        if (sprites == null || sprites.Length == 0)
            yield break;

        // 1) ��������Ʈ �� �� ���
        SoundManager.Instance.PlayUnpausableSFX("StartSFX");
        foreach (var s in sprites)
        {
            targetImage.sprite = s;
            yield return new WaitForSecondsRealtime(frameDuration);
        }

        // 2) ���̵� �ƿ�
        if (fadeController != null)
            yield return StartCoroutine(fadeController.FadeOutCoroutine());  // :contentReference[oaicite:0]{index=0}


        // 4) �� ��ȯ
        SceneManager.Instance.LoadScene(sceneName);
    }

    private void StopCurrent()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
}