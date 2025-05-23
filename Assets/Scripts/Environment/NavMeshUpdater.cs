using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] float updateRate = 0.5f;
    public NavMeshSurface Surface2D;

    private bool needsUpdate = false;

    void OnEnable()
    {
        StartCoroutine(UpdateNavMeshRoutine());
    }

    public void RequestUpdate()
    {
        needsUpdate = true;
    }

    IEnumerator UpdateNavMeshRoutine()
    {
        while (true)
        {
            if (needsUpdate)
            {
                UpdateNavMeshInBounds();
                needsUpdate = false;
            }
            yield return new WaitForSeconds(updateRate);
        }
    }

    void UpdateNavMeshInBounds()
    {
        if (Surface2D != null)
        {
            Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        }
    }
}
