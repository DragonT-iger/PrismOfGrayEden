using UnityEngine;

public class PauseZoneSizeTrigger : MonoBehaviour
{
    [SerializeField] PauseZoneSizeControl pauseZoneSizeControlScript;
    [SerializeField] float targetSize = 10.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pauseZoneSizeControlScript.PauseZoneColliderTrigger(targetSize);
        }
    }
}
