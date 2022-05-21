using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTestWalk : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    IEnumerator Start()
    {
        while (true)
        {
            navMeshAgent.SetDestination(new Vector3(
                RNG.Range(16f, -16f), 2f, RNG.Range(-36f, 0f)));

            yield return new WaitForSeconds(RNG.Range(2f, 4f));
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < navMeshAgent.path.corners.Length-1; i++)
        {
            Gizmos.DrawLine(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }
    }
}
