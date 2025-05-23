using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteDissolve : MonoBehaviour
{
    [Header("연출 설정")]
    [SerializeField] float dissolveDuration = 2f;
    [Header("낮출수록 파티클 ↑, 높일수록 파티클 ↓")]
    [SerializeField] int resolution = 4;
    [Header("캐릭터 반경 내 파티클 흩뿌릴 범위")]
    [SerializeField] float scatterRadius = 3f; // 
    [SerializeField] float yOffsetCorrection = 0.125f;
    [Header("파티클 색상 조정")]
    [SerializeField] Color particleColor = Color.black; // DDD9B9 색깔코드
    // 이미지가 5개야 머리 몸통 다리 다리 손 손  name이 오른손이다 하면 sprite 이런식으로 for문을 돌리는데 그걸 함수로 가지고 있고 한번만 호출해주면
    // 원래 있던 이미지로 돌아갈려면 추가적으로 배열로 
    private SpriteRenderer spriteRenderer;
    private bool isAnimating = false;
    private bool isDissolving = false;

    PlayerMovement playerMoveScript;
    void Start() 
    {
        playerMoveScript = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //----------------- 재생성. 세이브포인트에서 다시 살아날 때 사용할거 ----------------
    public void StartReconstruct() 
    {
        if (isAnimating) return;
        isAnimating = true;

        if (TryGetComponent<Animator>(out var animator))
            animator.enabled = false;

        spriteRenderer.enabled = false;

        StartCoroutine(ReconstructFromParticles());
    }
    IEnumerator ReconstructFromParticles() 
    {
        Sprite sprite = spriteRenderer.sprite;
        Texture2D tex = sprite.texture;
        Rect texRect = sprite.textureRect;
        float ppu = sprite.pixelsPerUnit;

        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        // 유효 알파 영역 찾기
        for (int y = 0; y < texRect.height; y += resolution)
        {
            for (int x = 0; x < texRect.width; x += resolution)
            {
                Color px = tex.GetPixel((int)texRect.x + x, (int)texRect.y + y);
                if (px.a < 0.1f) continue;

                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }
        }

        int width = maxX - minX;
        int height = maxY - minY;

        // 중심 오프셋 계산 + y축 미세 보정
        float offsetX = (minX + width / 2f - sprite.pivot.x) / ppu;
        float offsetY = (minY + height / 2f - sprite.pivot.y) / ppu + yOffsetCorrection;

        List<GameObject> particles = new List<GameObject>();

        for (int y = minY; y <= maxY; y += resolution)
        {
            for (int x = minX; x <= maxX; x += resolution)
            {
                Color px = tex.GetPixel((int)texRect.x + x, (int)texRect.y + y);
                if (px.a < 0.1f) continue;

                float localX = (x - sprite.pivot.x) / ppu - offsetX;
                float localY = (y - sprite.pivot.y) / ppu - offsetY;

                if (spriteRenderer.flipX) localX = -localX;
                if (spriteRenderer.flipY) localY = -localY;

                Vector3 targetLocalPos = new Vector3(localX, localY, 0f);
                Vector3 targetWorldPos = spriteRenderer.transform.TransformPoint(targetLocalPos);

                // 랜덤 흩뿌리기
                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * scatterRadius;
                Vector3 startPos = targetWorldPos + new Vector3(randomOffset.x, randomOffset.y, 0f);

                float dotSize = Mathf.Min(1f / ppu, 1f / ppu) * resolution * transform.localScale.x;

                GameObject dot = CreateDot(startPos, dotSize * 0.5f);
                dot.transform.position = startPos;
                dot.transform.SetParent(transform, true);
                particles.Add(dot);

                float delay = UnityEngine.Random.Range(0f, 0.5f);
                StartCoroutine(AnimateDotIn(dot, startPos, targetWorldPos, delay, dotSize));
            }
        }

        yield return new WaitForSeconds(dissolveDuration + 0.6f);

        foreach (var p in particles)
        {
            if (p != null) Destroy(p);
        }

        spriteRenderer.enabled = true;
        if (TryGetComponent<Animator>(out var animator))
        {
            animator.enabled = true;
            
        }
    }

    IEnumerator AnimateDotIn(GameObject dot, Vector3 start, Vector3 end, float delay, float dotSize) 
    {
        yield return new WaitForSeconds(delay);
        if (dot == null) yield break;

        float duration = dissolveDuration * 0.5f;
        float time = 0f;

        Renderer r = dot.GetComponent<Renderer>();
        Color c0 = particleColor;
        c0.a = 0f;

        while (time < duration)
        {
            if (dot == null) yield break;

            float t = time / duration;
            dot.transform.position = Vector3.Lerp(start, end, t);
            dot.transform.localScale = Vector3.Lerp(Vector3.one * dotSize * 0.5f, Vector3.one * dotSize, t);

            Color c = Color.Lerp(c0, particleColor, t);
            c.a = Mathf.Lerp(0f, 1f, t);
            r.material.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        if (dot != null)
        {
            dot.transform.position = end;
            dot.transform.localScale = Vector3.one * dotSize;
        }
    }


    //----------------- 사라지기. 죽는 경우 사용할거 ----------------
    public void StartDissolve() 
    {
        if (isDissolving) return;
        isDissolving = true;

        if (TryGetComponent<Animator>(out var animator))
            animator.enabled = false;

        spriteRenderer.enabled = false;

        StartCoroutine(DissolveToParticles());
    }


    IEnumerator DissolveToParticles() 
    {
        Sprite sprite = spriteRenderer.sprite;
        Texture2D tex = sprite.texture;
        Rect texRect = sprite.textureRect;
        float ppu = sprite.pixelsPerUnit;

        int minX = (int)texRect.width, maxX = 0;
        int minY = (int)texRect.height, maxY = 0;

        // 알파가 있는 픽셀만 영역 계산 (중심 잡기용)
        for (int y = 0; y < texRect.height; y++)
        {
            for (int x = 0; x < texRect.width; x++)
            {
                Color px = tex.GetPixel((int)texRect.x + x, (int)texRect.y + y);
                if (px.a > 0.1f)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        float centerX = (minX + maxX) * 0.5f;
        float centerY = (minY + maxY) * 0.5f;

        List<GameObject> particles = new List<GameObject>();

        for (int y = 0; y < texRect.height; y += resolution)
        {
            for (int x = 0; x < texRect.width; x += resolution)
            {
                Color px = tex.GetPixel((int)texRect.x + x, (int)texRect.y + y);
                if (px.a < 0.1f) continue;

                float localX = (x - centerX) / ppu;
                float localY = (y - centerY) / ppu;

                if (spriteRenderer.flipX) localX = -localX;
                if (spriteRenderer.flipY) localY = -localY;

                Vector3 localPos = new Vector3(localX, localY, 0f);
                Vector3 worldPos = spriteRenderer.transform.TransformPoint(localPos);

                float dotSize = Mathf.Min(1f / ppu, 1f / ppu) * resolution * transform.localScale.x;
                GameObject dot = CreateDot(worldPos, dotSize);
                dot.transform.SetParent(this.transform, true);
                particles.Add(dot);

                float delay = 1f - (y / (float)(texRect.height - 1));
                StartCoroutine(AnimateDotOut(dot, delay));
            }
        }

        yield return new WaitForSeconds(dissolveDuration + 0.5f);

        foreach (var d in particles)
            if (d) Destroy(d);


        SceneManager.Instance.LoadSavePointScene();
    }
    IEnumerator AnimateDotOut(GameObject dot, float delay) 
    {
        yield return new WaitForSeconds(delay);

        Vector3 start = dot.transform.position;
        Vector3 end = start + new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(1.2f, 2.0f),
            0f
        );

        Renderer r = dot.GetComponent<Renderer>();
        Color c0 = r.material.color;

        float t = 0f;
        while (t < dissolveDuration)
        {
            float frac = t / dissolveDuration;

            dot.transform.position = Vector3.Lerp(start, end, frac);

            Color c = c0;
            c.a = Mathf.Lerp(1f, 0f, frac);
            r.material.color = c;

            t += Time.deltaTime;
            yield return null;
        }

        if (dot) Destroy(dot);
    }

    // 공용으로 쓰는거
    GameObject CreateDot(Vector3 pos, float size) 
    {
        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Quad);
        dot.transform.position = pos;
        dot.transform.localScale = Vector3.one * size;

        var rd = dot.GetComponent<Renderer>();
        rd.material = new Material(Shader.Find("Unlit/Color"));
        rd.material.color = particleColor;

        return dot;
    }


}
