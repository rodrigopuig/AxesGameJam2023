using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObstacle : MonoBehaviour
{
    public AnimationCurve curve;
    public float timeToGrow;
    public float maxRadius;

    public CapsuleCollider physicalCapsuleCollider;

    [Header("Component")]
    public Transform trSprite;
    public bool applyMaxScale;
    public float maxScale;

    [Header("Gizmos")]
    public bool drawGizmos;

    Vector2 originalSpriteScale;
    Vector2 maxSpriteScale;

    float radius;

    private void Awake()
    {
        physicalCapsuleCollider = GetComponent<CapsuleCollider>();
        radius = physicalCapsuleCollider.radius;

        originalSpriteScale = trSprite.localScale;

        if(applyMaxScale)
        {
            float _scale = Mathf.Clamp(maxRadius / radius, 0.1f, maxScale);
            maxSpriteScale = originalSpriteScale * maxScale;
        }
        else
        {
            maxSpriteScale = originalSpriteScale * (maxRadius / radius);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float _counter = 0;
        float _lerpValue = 0;
        while(_counter < timeToGrow)
        {
            _counter += Time.deltaTime;
            _lerpValue = curve.Evaluate(_counter / timeToGrow);
            physicalCapsuleCollider.radius = Mathf.Lerp(radius, maxRadius, _lerpValue);
            trSprite.localScale = Vector2.Lerp(originalSpriteScale, maxSpriteScale, _lerpValue);
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
