// PauseZoneTrigger.cs
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class PauseZoneTrigger : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        // 나간 오브젝트에 붙은 모든 MonoBehaviour를 검사
        foreach (var mb in other.GetComponents<MonoBehaviour>())
            if (mb is IPausable p)
                p.Pause();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var mb in other.GetComponents<MonoBehaviour>())
            if (mb is IPausable p)
                p.Resume();
    }
}
