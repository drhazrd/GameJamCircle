using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioSource sourceSFX;
    private void Awake()
    {
        if (instance != this && AudioManager.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayClip(AudioClip clip)
    {
        SoundRandomizer();
        sourceSFX.PlayOneShot(clip);
    }
    private void SoundRandomizer()
    {
        sourceSFX.pitch = UnityEngine.Random.Range(.75f, 1.25f);
        sourceSFX.volume = UnityEngine.Random.Range(.85f, 1f);
    }
}
