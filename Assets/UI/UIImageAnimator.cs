using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIImageAnimator : MonoBehaviour
{
    [Header("애니메이션 대상 Image")]
    [SerializeField] private Image targetImage;

    [Header("기본 반복 애니메이션")]
    [SerializeField] private Sprite[] loopSprites;

    [Header("한 번만 재생할 애니메이션")]
    [SerializeField] private Sprite[] oneShotSprites;

    [Header("프레임 간 대기 시간(초)")]
    [SerializeField] private float frameDuration = 0.2f;

    [Header("페이드 아웃 컨트롤러")]
    [SerializeField] private PlayUI fadeController;

    private Coroutine currentCoroutine;
    public bool hasPlayedOneShot = false;  // ← 한 번만 재생 플래그

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (fadeController == null)
            Debug.LogError("PlayUI(Fade 컨트롤러)가 할당되지 않음");
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
    /// one-shot 애니메이션 재생 후 페이드 & 씬 전환
    /// 원하는 씬 이름을 파라미터로 전달
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

        // 1) 스프라이트 한 번 재생
        SoundManager.Instance.PlayUnpausableSFX("StartSFX");
        foreach (var s in sprites)
        {
            targetImage.sprite = s;
            yield return new WaitForSecondsRealtime(frameDuration);
        }

        // 2) 페이드 아웃
        if (fadeController != null)
            yield return StartCoroutine(fadeController.FadeOutCoroutine());  // :contentReference[oaicite:0]{index=0}


        // 4) 씬 전환
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