using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class StageTrigger : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    [SerializeField] private string nextSceneName = "Stage2_1";

    private Collider2D _col;

    private void Awake()
    {
        // Collider2D 컴포넌트 가져오고 Trigger 모드로 설정
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player 태그가 닿았을 때만 씬 전환
        if (other.CompareTag("Player"))
        {
            SceneManager.Instance.LoadScene(nextSceneName);
        }
    }
}
