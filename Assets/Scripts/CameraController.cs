using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance = null;

    public float lookAhead;
    public float smooth;

    [Header("Shake")]
    public Transform trShake;
    public AnimationCurve shakeAmplitude;
    public float maxAmplitude;
    public float maxShakeTime;
    public int framesPerCameraChange = 1;

    private CarController car;
    float shakeCounter;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        car = FindObjectOfType<CarController>();
    }

    void LateUpdate()
    {
        if(car.rb != null)
        {
            transform.position = Vector3.Lerp(transform.position, car.transform.position + car.rb.velocity * lookAhead, smooth);
        }
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float _lerpValue = 0;
        shakeCounter = 0;
        while(shakeCounter < maxShakeTime)
        {
            shakeCounter += Time.deltaTime;
            _lerpValue = shakeAmplitude.Evaluate(shakeCounter / maxShakeTime);
            trShake.localPosition = Random.insideUnitSphere * _lerpValue * maxAmplitude;

            for(int i = 0; i<framesPerCameraChange; i++)
            yield return null;
        }

        trShake.localPosition = Vector3.zero;
    }
}
