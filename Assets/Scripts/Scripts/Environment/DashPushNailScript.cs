using Unity.VisualScripting;
using UnityEngine;

public class DashPushNailScript : MonoBehaviour, IPausable
{
    [SerializeField] float nailPushDistance = 0.5f;
    [SerializeField] float returnTime = 1.0f;
    
    [SerializeField] private GameObject extendableParts;
    [SerializeField] private float _extendablePartExtendDistance; // 늘어날 거리
    NavMeshUpdater navMeshUpdater;
    public bool IsPushed = false;
    private bool _isGoingBack;
    // Vec2로 변경했습니다.
    [SerializeField] Vector2 defaultPos;
    [SerializeField] Vector2 pushedPos;
    [SerializeField] private Vector3 _extendablePartDefaultPos;
    [SerializeField] private Vector2 _extendablePartPushedPos;

    float elapesedTime;
    bool isPaused = true;
    // 여기서부터 제가 추가한 변수입니다.
    [SerializeField] private DashPushTailScript _dashPushTailScript;
    [SerializeField] private Vector3 _nailMoveDirection;
    private Vector3 _destinationPosition;
    private Vector3 _tailDestinationPosition;
    [SerializeField] private Sprite[] _springSprites;
    [SerializeField] private SpriteRenderer _springRenderer;
    private int currentSpriteIndex = 0;

    public Vector2 VelocityBeforePause { get; private set; } = Vector2.zero;

    public bool IsPaused { get => isPaused; }

    public void Pause()
    {
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }
    void Start()
    {
        defaultPos = transform.position;
        _extendablePartDefaultPos = extendableParts.transform.localPosition;
        // _nailMoveDirection에 따라 _destinationPosition 설정
        if (_nailMoveDirection == Vector3.left)
        {
            _destinationPosition = new Vector3(defaultPos.x - nailPushDistance, defaultPos.y, transform.position.z);
            _tailDestinationPosition = new Vector3(_extendablePartDefaultPos.x - _extendablePartExtendDistance, _extendablePartDefaultPos.y, _extendablePartDefaultPos.z);
        }
        else if (_nailMoveDirection == Vector3.right)
        {
            _destinationPosition = new Vector3(defaultPos.x + nailPushDistance, defaultPos.y, transform.position.z);
            _tailDestinationPosition = new Vector3(_extendablePartDefaultPos.x + _extendablePartExtendDistance, _extendablePartDefaultPos.y, _extendablePartDefaultPos.z);
        }
        else if (_nailMoveDirection == Vector3.up)
        {
            _destinationPosition = new Vector3(defaultPos.x, defaultPos.y + nailPushDistance, transform.position.z);
            _tailDestinationPosition = new Vector3(_extendablePartDefaultPos.x, _extendablePartDefaultPos.y + _extendablePartExtendDistance, _extendablePartDefaultPos.z);
        }
        else if (_nailMoveDirection == Vector3.down)
        {
            _destinationPosition = new Vector3(defaultPos.x, defaultPos.y - nailPushDistance, transform.position.z);
            _tailDestinationPosition = new Vector3(_extendablePartDefaultPos.x, _extendablePartDefaultPos.y - _extendablePartExtendDistance, _extendablePartDefaultPos.z);
        }
        else
        {
            // 방향이 지정되지 않은 경우 기본값 설정
            _destinationPosition = new Vector3(defaultPos.x, defaultPos.y, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            if (IsPushed && elapesedTime < returnTime)
            {
                // 스프링 렌더러에 적절한 시간마다 스프라이트 교체가 필요합니다.
                _dashPushTailScript.IsThisPushingNow = true;
                elapesedTime += Time.fixedDeltaTime;
                transform.position = Vector2.Lerp(defaultPos, _destinationPosition, elapesedTime);
                extendableParts.transform.localPosition = Vector2.Lerp(_extendablePartDefaultPos, _tailDestinationPosition, elapesedTime);
                pushedPos = transform.position;
                _extendablePartPushedPos = extendableParts.transform.localPosition;
            }
            else if (IsPushed && _isGoingBack)
            {
                // 스프링 렌더러에 적절한 시간마다 스프라이트 교체가 필요합니다.
                // Vec2로 변경했습니다.
                elapesedTime += Time.fixedDeltaTime;
                transform.position = Vector2.Lerp(pushedPos, defaultPos, elapesedTime - returnTime);
                extendableParts.transform.localPosition = Vector2.Lerp(_extendablePartPushedPos, _extendablePartDefaultPos, elapesedTime - returnTime);
                if (Vector2.Distance(transform.position, defaultPos) < 0.01f)
                {
                    elapesedTime = default;
                    IsPushed = false;
                    _isGoingBack = false;
                }
            }
            else if (_isGoingBack == false && elapesedTime >= returnTime)
            {
                _dashPushTailScript.IsThisPushingNow = false;
                _isGoingBack = true;
            }
            else if (IsPushed == false)
            {
                elapesedTime = default;
            }
        }
    }

    private void Update()
    {
        if (IsPushed)
        {
            currentSpriteIndex = (int)((Vector3.Distance(defaultPos, transform.position) / Vector3.Distance(defaultPos, _destinationPosition)) * _springSprites.Length) - 1;
            if (currentSpriteIndex < 0)
            {
                currentSpriteIndex = 0;
            }
            transform.Find("Spring").GetComponent<SpriteRenderer>().sprite = _springSprites[currentSpriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void SetSavedVelocity(Vector2 input)
    {
        return;
    }
}