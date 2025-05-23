using UnityEngine;
using System.Collections;

public class PauseZoneSizeControl : MonoBehaviour
{
    [SerializeField] float pauseZoneColliderRadiusMinimumsize = 1.0f;
    [SerializeField] float pauseZoneColliderShrinkTime = 0.2f;
    [SerializeField] float pauseZoneColliderMinimumStayTime = 0.2f;
    [SerializeField] float pauseZoneColliderExpandTime = 0.5f;

    CircleCollider2D pauseZoneCollider;
    float pauseZoneColliderRadiusDefaultSize;
    float currentPauseZoneColliderRadius = 0.0f;
    float shrinkAndExpandTimeElapsed = 0.0f;
    public bool isShrinkingStart = false;
    bool isShrinking = false;
    void Start()
    {
        pauseZoneCollider = GetComponent<CircleCollider2D>();
        pauseZoneColliderRadiusDefaultSize = pauseZoneCollider.radius;
    }

    void Update()
    {
        if (isShrinkingStart)
        {
            currentPauseZoneColliderRadius = pauseZoneCollider.radius;
            shrinkAndExpandTimeElapsed = 0.0f;
            isShrinkingStart = false;
            isShrinking = true;
        }
        if (isShrinking)
        {
            PauseZoneColliderShrinkAndExpand();
        }
    }
    void PauseZoneColliderShrinkAndExpand()
    {
        shrinkAndExpandTimeElapsed += Time.deltaTime;
        if (shrinkAndExpandTimeElapsed < pauseZoneColliderShrinkTime)
        {
            pauseZoneCollider.radius = Mathf.SmoothStep(currentPauseZoneColliderRadius, pauseZoneColliderRadiusMinimumsize, shrinkAndExpandTimeElapsed / pauseZoneColliderShrinkTime);
        }
        else if (shrinkAndExpandTimeElapsed < pauseZoneColliderShrinkTime + pauseZoneColliderMinimumStayTime)
        {

        }
        else if (shrinkAndExpandTimeElapsed < pauseZoneColliderShrinkTime + pauseZoneColliderMinimumStayTime + pauseZoneColliderExpandTime)
        {
            pauseZoneCollider.radius = Mathf.SmoothStep(pauseZoneColliderRadiusMinimumsize, pauseZoneColliderRadiusDefaultSize, (shrinkAndExpandTimeElapsed - pauseZoneColliderShrinkTime - pauseZoneColliderMinimumStayTime) / pauseZoneColliderExpandTime);
        }
        else
        {
            isShrinking = false;
        }
    }

    public void PauseZoneColliderTrigger(float targetSize)
    {
        StartCoroutine(PauseZoneColliderChangeSize(targetSize));
    }

    IEnumerator PauseZoneColliderChangeSize(float targetSize)
    {
        float currentTime = 0.0f;
        while (currentTime < 5.0f)
        {
            currentTime += Time.deltaTime;
            pauseZoneColliderRadiusDefaultSize = Mathf.SmoothStep(pauseZoneColliderRadiusDefaultSize, targetSize, currentTime / 5);
            pauseZoneCollider.radius = pauseZoneColliderRadiusDefaultSize;
            yield return null;
        }
    }
}
