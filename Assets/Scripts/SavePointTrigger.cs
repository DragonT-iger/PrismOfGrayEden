// SavePointTrigger.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SavePointTrigger : MonoBehaviour
{
    [Header("이 트리거가 활성화할 SavePoint")]
    [SerializeField] private SavePoint triggerPoint;

    private void Reset()
    {
        // Collider2D를 Trigger로 자동 설정
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
