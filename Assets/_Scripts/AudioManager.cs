using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioClip gameMusic;
    public AudioSource BGM;
    public AudioSource SFX;
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
        if(gameMusic != null) PlayBGM(gameMusic);
    }

    public void PlayBGM(AudioClip clip)
    {
        BGM.clip = clip;
    }
    public void PlaySFXClip(AudioClip clip)
    {
        SoundRandomizer();
        SFX.PlayOneShot(clip);
    }
    private void SoundRandomizer()
    {
        SFX.pitch = UnityEngine.Random.Range(1f, 1.25f);
        SFX.volume = UnityEngine.Random.Range(.95f, 1f);
    }
}
