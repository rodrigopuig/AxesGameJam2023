using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStation : MonoBehaviour
{
    public Transform[] patrolPoints;
    public PatrolEntity patrolEntity;

    [Header("Gizmos")]
    public bool drawGizmos = true;

    private void Awake()
    {
        patrolEntity.SetTargets(patrolPoints);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.red;

        for(int i = 0; i< patrolPoints.Length-1; i++)
        {
            Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i+1].position);
        }

        Gizmos.DrawSphere(patrolPoints[patrolPoints.Length - 1].position, 0.2f);
        Gizmos.DrawLine(patrolPoints[0].position, patrolPoints[patrolPoints.Length - 1].position);

    }                   
}
