using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObstacle : MonoBehaviour
{
    public AnimationCurve curve;
    public float timeToGrow;
    public float maxRadius;

    public CapsuleCollider physicalCapsuleCollider;

    [Header("Gizmos")]
    public bool drawGizmos;

    

    float radius;

    private void Awake()
    {
        physicalCapsuleCollider = GetComponent<CapsuleCollider>();
        radius = physicalCapsuleCollider.radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float counter = 0;
        while(counter < timeToGrow)
        {
            counter += Time.deltaTime;
            physicalCapsuleCollider.radius = Mathf.Lerp(radius, maxRadius, curve.Evaluate(counter / timeToGrow));
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, physicalCapsuleCollider.radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
