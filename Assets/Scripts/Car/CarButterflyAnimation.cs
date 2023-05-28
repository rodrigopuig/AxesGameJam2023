using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarButterflyAnimation : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public float time;
    public Transform [] trWings; //0 left 1 right

    public float leftwing_up = -75f;
    public float leftwing_down = 10;

    public float rightwing_up = 75;
    public float rightwing_down = -10;

    public ParticleSystem completeComboPs;

    public void Wing(int _donothing)
    {
        StopAllCoroutines();
        StartCoroutine(DoWingAnimation());
    }

    IEnumerator DoWingAnimation()
    {
        float _lerp = 0,
            _counter = 0;
        while(_counter < time)
        {
            _counter += Time.deltaTime;
            _lerp = animationCurve.Evaluate(_counter / time);
            trWings[0].localRotation = Quaternion.Euler(0, Mathf.Lerp(leftwing_up, leftwing_down, _lerp), 0);
            trWings[1].localRotation = Quaternion.Euler(0, Mathf.Lerp(rightwing_up, rightwing_down, _lerp), 0);
            yield return null;
        }
    }

    public void CompleteCombo()
    {
        completeComboPs.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        completeComboPs.Play(true);
        
        SoundController.instance?.PlaySound(SFXid.mariposa);
    }
}
