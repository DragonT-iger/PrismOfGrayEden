using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class StageTrigger : MonoBehaviour
{
    [Header("�̵��� �� �̸�")]
    [SerializeField] private string nextSceneName = "Stage2_1";

    private Collider2D _col;

    private void Awake()
    {
        // Collider2D ������Ʈ �������� Trigger ���� ����
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player �±װ� ����� ���� �� ��ȯ
        if (other.CompareTag("Player"))
        {
            SceneManager.Instance.LoadScene(nextSceneName);
        }
    }
}
