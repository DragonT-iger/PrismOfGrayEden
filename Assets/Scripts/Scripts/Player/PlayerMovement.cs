using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Sprite;
using UnityEngine.Splines;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour, IYPositionBroadcastable
{
    public float moveSpeed = 5f;
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;

    [SerializeField] private GameObject dashSlider;
    [SerializeField] private GameObject shrinkDashSlider;
    private SlicedFilledImage dashSliderValue;
    private SlicedFilledImage shrinkDashSliderValue;

    [Header("대시 설정")]
    [SerializeField] float dashCooltimeDefault = 1.0f;
    [SerializeField] float shrinkDashCooltimeDefault = 3.0f;
    float currentDashCooltime = 0.0f;
    float currentShrinkDashCooltime = 0.0f;

    public Tilemap walkableTilemap;
    public Tilemap brokenFloorTilemap;

    [SerializeField] Sprite stunnedUpSprite;
    [SerializeField] Sprite stunnedDownSprite;
    [SerializeField] Sprite stunnedLeftSprite;
    [SerializeField] Sprite stunnedRightSprite;

    SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    Vector2 input;
    private Vector2 lastMoveDirection = Vector2.right;

    private Vector2 dashDirection;   // 대시 시작 시 고정되는 방향
    [SerializeField] float knockbackDistance = 0.4f;    // 넉백 크기값 조절
    bool isDashing;
    bool isStunned;

    [SerializeField] string[] dashWallCollideTag;
    [SerializeField] GameObject dashCollideEffect;
    [SerializeField] GameObject playerCamera;
    Vector3 cameraDefaultPos;
    [SerializeField] float cameraShakeDuration = 1.0f;
    [SerializeField] float cameraShakeMagnitude = 1.0f;
    float currentCameraShakeDuration = 0.0f;
    bool isNailDashed = false;

    int playerLayer;
    int brokenFloorLayer;

    Animator aniController;
    
    readonly string idleSpeedParam = "IdleSpeed";
    readonly string walkSpeedParam = "WalkSpeed";
    readonly string dashSpeedParam = "DashSpeed";

    float footStepSoundRate = 0.7f;
    float footStepSoundTimer = 0.0f;

    NavMeshUpdater navMeshUpdater;

    public bool dissolving = false;
    public bool blockInput = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aniController = GetComponent<Animator>();
        navMeshUpdater = Object.FindFirstObjectByType<NavMeshUpdater>();

        playerLayer = LayerMask.NameToLayer("Player");
        brokenFloorLayer = LayerMask.NameToLayer("BrokenFloor");

        cameraDefaultPos = playerCamera.transform.localPosition;

        currentDashCooltime = dashCooltimeDefault;
        currentShrinkDashCooltime = shrinkDashCooltimeDefault;

        if (dashSlider == null || shrinkDashSlider == null)
        {
            Debug.LogWarning("DashSlider들을 할당하세용");
        }
        else
        {
            dashSliderValue = dashSlider.GetComponent<SlicedFilledImage>();
            shrinkDashSliderValue = shrinkDashSlider.GetComponent<SlicedFilledImage>();
        }
        isStunned = false;

        rb.linearDamping = 50f;
    }
    /*float test = 3f;
    bool finish = false;*/
    void Update()
    {
        if (dissolving) return;
        if (blockInput) return;
        // 공용 타이머 처리 & 카메라 흔들림
        TimeManage();
        CameraShake();
        UpdateAnimationParameters();
        UpdateDashSlider();

        // 플레이어 소팅 오더 전역 변수화
        PlayerYPositionBroadCaster.SetPublicPlayerSortingOrder(this, transform.position.y);

        if (isDashing || isStunned) return;

        /*if (footStepSoundTimer > footStepSoundRate)
        {
            SoundManager.Instance.PlaySFX("PlayerFootStep");
            footStepSoundTimer = 0f;
        }*/
        /*test -= Time.deltaTime;
        if (test < 0 && !finish)
        {
            finish = true;
            *//*SpriteDissolve sp = GetComponent<SpriteDissolve>();
            sp.StartReconstruct();*//*
            SpriteCutScene.Instance.StartCutScene(1);
        }*/
        // ────────────────── 입력 처리 ──────────────────
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        if (input != Vector2.zero)
            lastMoveDirection = input;

        if (input.x < 0)
            spriteRenderer.flipX = true;
        else if (input.x > 0)
            spriteRenderer.flipX = false;

        DashCooltimeControl();
    }

    void FixedUpdate()
    {
        if (blockInput || isDashing || isStunned || input == Vector2.zero) return;

        Vector2 targetPos = rb.position + input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    //──────────────────────────────────────────────────────────────────────
    //                               Utility
    //──────────────────────────────────────────────────────────────────────
    void TimeManage()
    {
        currentDashCooltime += Time.deltaTime;
        currentShrinkDashCooltime += Time.deltaTime;
        currentCameraShakeDuration -= Time.deltaTime;
        if (input != Vector2.zero)
        {
            footStepSoundTimer += Time.deltaTime;
        }
    }

    void CameraShake()
    {
        if (currentCameraShakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitCircle * Mathf.SmoothStep(0.0f, cameraShakeMagnitude, currentCameraShakeDuration / cameraShakeDuration);
            playerCamera.transform.localPosition = cameraDefaultPos + shakeOffset;
        }
        else if (playerCamera.transform.localPosition != cameraDefaultPos)
        {
            playerCamera.transform.localPosition = cameraDefaultPos;
        }
    }

    public void BlockInput()
    {
        blockInput = true;
    }

    public void EnableInput()
    {
        blockInput = false;
    }

    void DashCooltimeControl()
    {
        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector2 dashDir = input != Vector2.zero ? input : lastMoveDirection;

            bool emergencyDashKey = Input.GetKey(KeyCode.LeftControl);

            if (emergencyDashKey && currentShrinkDashCooltime > shrinkDashCooltimeDefault)
            {
                StartCoroutine(Dash(dashDir, true));
                currentShrinkDashCooltime = 0.0f;
            }
            else if (!emergencyDashKey && currentDashCooltime > dashCooltimeDefault)
            {
                StartCoroutine(Dash(dashDir, false));
                currentDashCooltime = 0.0f;
            }
        }
    }

    //──────────────────────────────────────────────────────────────────────
    //                            Animation
    //──────────────────────────────────────────────────────────────────────
    void UpdateAnimationParameters()
    {
        float moveMagnitude = input.magnitude;

        if (isDashing)
        {
            aniController.SetFloat(idleSpeedParam, 0);
            aniController.SetFloat(walkSpeedParam, 0);
            aniController.SetFloat(dashSpeedParam, 1);

            if (dashDirection.y > 0)
                aniController.SetFloat("yDirection", 1);
            else if (dashDirection.y < 0)
                aniController.SetFloat("yDirection", -1);
            else
                aniController.SetFloat("yDirection", 0); // side
        }
        else if (moveMagnitude > 0.1f)
        {
            aniController.SetFloat(idleSpeedParam, 0);
            aniController.SetFloat(walkSpeedParam, 1);
            aniController.SetFloat(dashSpeedParam, 0);

            if (input.y > 0)
                aniController.SetFloat("yDirection", 1);
            else if (input.y < 0)
                aniController.SetFloat("yDirection", -1);
            else
                aniController.SetFloat("yDirection", 0); // side
        }
        else
        {
            aniController.SetFloat(idleSpeedParam, 1);
            aniController.SetFloat(walkSpeedParam, 0);
            aniController.SetFloat(dashSpeedParam, 0);
        }
    }

    //──────────────────────────────────────────────────────────────────────
    //                               Dash
    //──────────────────────────────────────────────────────────────────────
    IEnumerator Dash(Vector2 direction, bool isEmergencyDash)
    {

        if (isEmergencyDash)
            SoundManager.Instance.PlaySFX("PlayerShrinkDash");
        /*else
            SoundManager.Instance.PlaySFX("PlayerDash");*/

        dashDirection = direction;
        Debug.Log($"dash direction {direction}");
        yield return new WaitForFixedUpdate();

        isDashing = true;
        if (isEmergencyDash)
            GetComponentInChildren<PauseZoneSizeControl>().isShrinkingStart = true;

        float elapsed = 0f;
        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + direction * dashDistance;

        Physics2D.IgnoreLayerCollision(playerLayer, brokenFloorLayer, true);

        while (elapsed < dashDuration)
        {
            Vector2 newPos = Vector2.Lerp(startPos, endPos, elapsed / dashDuration);
            rb.MovePosition(newPos);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Physics2D.IgnoreLayerCollision(playerLayer, brokenFloorLayer, false);
        isDashing = false;
        isNailDashed = false;
    }
    void UpdateDashSlider()
    {
        if (dashSlider != null)
        {
            dashSliderValue.fillAmount = Mathf.Clamp01(currentDashCooltime / dashCooltimeDefault);
        }
        if (shrinkDashSlider != null)
        {
            shrinkDashSliderValue.fillAmount = Mathf.Clamp01(currentShrinkDashCooltime / shrinkDashCooltimeDefault);
        }
    }

    IEnumerator Knockback(Vector2 direction) 
    {
        isStunned = true;
        isDashing = false; // 대시 종료

        // 1) 애니메이터를 Idle 상태로 강제 전환
        aniController.Play("Idle", 0, 0f);
        aniController.enabled = false;
        aniController.Update(0f);

        // 애니메이션 대신 Sprite 직접 설정
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            spriteRenderer.sprite = direction.x > 0 ? stunnedRightSprite : stunnedLeftSprite;
        }
        else
        {
            spriteRenderer.sprite = direction.y < 0 ? stunnedUpSprite : stunnedDownSprite;
        }

        // 이미지 전환
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            aniController.SetFloat("yDirection", 0f);    // 좌우 방향이므로 yDirection 0
        }
        else
        {
            aniController.SetFloat("yDirection", direction.y > 0 ? 1f : -1f);
        }

        float knockbackDuration = 0.2f;

        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + direction * knockbackDistance;

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            Vector2 newPos = Vector2.Lerp(startPos, endPos, elapsed / knockbackDuration);
            rb.MovePosition(newPos);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            aniController.SetFloat("yDirection", 0); // 좌우 (side)
        else
            aniController.SetFloat("yDirection", direction.y > 0 ? 1 : -1); // 위 or 아래

        yield return new WaitForSeconds(0.25f); // 약간의 추가 딜레이 후 입력 복구

        aniController.enabled = true;
        aniController.Update(1f);
        // 방향에 따라 yDirection 설정
      
        isStunned = false;
    }


    //──────────────────────────────────────────────────────────────────────
    //                               Collisions
    //──────────────────────────────────────────────────────────────────────
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDashing) return;

        foreach (string tag in dashWallCollideTag)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                DashToWall(collision.contacts[0].point);
                SoundManager.Instance.PlaySFX("PlayerDashWall");
                return;
            }
        }

        if (collision.gameObject.CompareTag("DoorDash"))
        {
            SoundManager.Instance.PlaySFX("WoodenDoorDash");
            DoorDash(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isDashing) return;

        if (!isNailDashed && collision.gameObject.tag == "Nail")
        {
            DashToNail(collision.gameObject);
            isNailDashed = true;
        }

        if (collision.gameObject.CompareTag("DoorDash"))
        {
            SoundManager.Instance.PlaySFX("WoodenDoorDash");
            DoorDash(collision.gameObject);
        }
    }

    void DashToWall(Vector2 point)
    {        
        if (isStunned) return; // 중복 방지

        GameObject effect = Instantiate(dashCollideEffect, point, Quaternion.identity);
        currentCameraShakeDuration = cameraShakeDuration;

        // 대시 반대 방향 계산
        Vector2 knockbackDirection = -dashDirection.normalized;
        StartCoroutine(Knockback(knockbackDirection));

        effect = Instantiate(dashCollideEffect, point, Quaternion.identity);
        currentCameraShakeDuration = cameraShakeDuration;
    }

    void DoorDash(GameObject door)
    {
        var col = door.GetComponent<Collider2D>();
        if (col) col.enabled = false;
        var anim = door.GetComponent<Animator>();
        if (anim) anim.SetTrigger("OpenTrigger");
        var mask = door.GetComponent<NavMeshPlus.Components.NavMeshModifier>();
        if (mask) mask.area = 0;
        if (navMeshUpdater != null)
        {
            navMeshUpdater.RequestUpdate();
        }
    }

    void DashToNail(GameObject nail)
    {
        nail.GetComponent<DashPushNailScript>().IsPushed = true;
        SoundManager.Instance.PlaySFX("PlayerHitNail");
    }
    //----------------------------------- Player Sound -------------------


    public void PlayerFootSFX() 
    {
        SoundManager.Instance.PlaySFX("PlayerFootStep");
    }
    public void PlayerDashSFX() 
    {
        SoundManager.Instance.PlaySFX("PlayerDash");
    }    
}
