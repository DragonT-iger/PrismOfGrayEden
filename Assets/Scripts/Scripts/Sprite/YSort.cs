using Assets.Scripts.Sprite;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    [SerializeField] private DoorBehaviorScript _doorBehaviourScript;
    private ISortingOrderReceiver _sortingOrderReceiver;
    private int offset = 0;
    [Tooltip("üũ�ϸ� Awake���� �� ���� sortingOrder�� �����մϴ�.")]
    public bool onlyOnce = false;
    
    [SerializeField] private float _yOffset;
    SpriteRenderer sr;
    private string _playerTag = "Player";
    private string _doorTag1 = "DoorVertical";
    private string _doorTag2 = "DoorDash";
    private int _currentOffset;

    void Awake()
    {
        if (_doorBehaviourScript != null)
        {
            _sortingOrderReceiver = _doorBehaviourScript as ISortingOrderReceiver;
        }
        sr = GetComponent<SpriteRenderer>();
        offset = sr.sortingOrder;

        if (onlyOnce)
        {
            // �� ���� ����ϰ� ��Ȱ��ȭ
            sr.sortingOrder = CalculateOrder();
            enabled = false;
        }
    }

    void LateUpdate()
    {
        sr.sortingOrder = CalculateOrder();
    }

    int CalculateOrder()
    {
        if (PlayerYPositionBroadCaster.PlayerYPosition < transform.position.y + _yOffset)
        {
            _currentOffset = -offset;
            if (_sortingOrderReceiver != null)
            {
                _sortingOrderReceiver.OnSortingOrderChanged(_currentOffset);
            }
            return _currentOffset;
        }
        _currentOffset = offset;
        if (_sortingOrderReceiver != null)
        {

            _sortingOrderReceiver.OnSortingOrderChanged(_currentOffset);
        }
        return _currentOffset;
    }
}
