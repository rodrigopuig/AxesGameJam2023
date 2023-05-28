using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    public AudioSource sourceSFX;
    public AudioSource constantSFX;
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
                int index = Random.Range(0, sfx.clips.Length);
                AudioClip clip = sfx.clips[index];
                sourceSFX.PlayOneShot(clip, sfx.volume);
                break;
            }
        }
    }

    public void UpdateRunVolume(float volume)
    {
        constantSFX.volume = volume;
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
    countdown321,
    countdown0,
    nabo,
    aplastar,
}

[System.Serializable]
public class SFX
{
    public SFXid id;
    public float volume = 1;
    // public bool loop;
    public AudioClip[] clips;

}
