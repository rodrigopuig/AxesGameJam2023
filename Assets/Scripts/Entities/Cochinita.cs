using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cochinita : PatrolEntity
{
    public InsectLeg[] legs;
    public float legSpeed = 90;
    public ParticleSystem hitPS;

    float auxiliarAngle;
    float outputAngle;

    private void Awake()
    {
        base.Awake();

        for (int i = 0; i < legs.Length; i++)
        {
            legs[i].angleOffset = legs[i].angleOffset * Mathf.Deg2Rad;
        }
    }

    private void Update()
    {
        base.Update();

        auxiliarAngle = Time.time * legSpeed * Mathf.Deg2Rad;
        //update legs
        for(int i = 0; i<legs.Length; i++)
        {
            outputAngle = -75f + 10f * Mathf.Sin(auxiliarAngle + legs[i].angleOffset);
            legs[i].trLeg.localRotation = Quaternion.Euler(0, 0, outputAngle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            isDead = true;

            deathDirection = Vector3.ProjectOnPlane(transform.position - other.transform.position, Vector3.up).normalized;

            //ps
            CapsuleCollider _capsuleCollider = GetComponent<CapsuleCollider>();
            hitPS.transform.position = transform.position - deathDirection * _capsuleCollider.radius;
            hitPS.Play(true);

            ChangeState(State.StateId.Die);
            SoundController.instance?.PlaySound(SFXid.aplastar);
        }
    }

    [System.Serializable]
    public struct InsectLeg
    {
        public Transform trLeg;
        public float angleOffset;
    }
}
