// SavePointTrigger.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SavePointTrigger : MonoBehaviour
{
    [Header("�� Ʈ���Ű� Ȱ��ȭ�� SavePoint")]
    [SerializeField] private SavePoint triggerPoint;

    private void Reset()
    {
        // Collider2D�� Trigger�� �ڵ� ����
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetSavePoint(triggerPoint);
        }
    }
}
