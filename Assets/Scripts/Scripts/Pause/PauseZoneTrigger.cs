// PauseZoneTrigger.cs
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class PauseZoneTrigger : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        // ���� ������Ʈ�� ���� ��� MonoBehaviour�� �˻�
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
