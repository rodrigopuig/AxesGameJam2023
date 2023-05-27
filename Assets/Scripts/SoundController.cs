using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    public AudioSource sourceSFX;
    public AudioSource sourceMusic;
    public SFX[] sounds;

    void Awake()
    {
        instance = this;
    }

    public void PlaySound(SFXid id)
    {
        foreach(var sfx in sounds)
        {
            if(sfx.id == id)
            {
                AudioClip clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
                sourceSFX.PlayOneShot(clip);
                break;
            }
        }
    }
}

public enum SFXid
{
    cochinilla,
    escarabajo,
    mariposa,
    polvito,
    boton,
    chocazo,
    meta,
    countdown,
}

[System.Serializable]
public class SFX
{
    public SFXid id;
    public AudioClip[] clips;

}
