using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioMixer aMixer;
    public AudioClip gameMusic;

    public AudioSource sourceSFX;
    public AudioSource sourceBGM;
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
        sourceBGM.clip = clip;
    }
    public void PlaySFXClip(AudioClip clip)
    {
        SoundRandomizer();
        sourceSFX.PlayOneShot(clip);
    }

    private void SoundRandomizer()
    {
        sourceSFX.pitch = UnityEngine.Random.Range(1f, 1.25f);
        sourceSFX.volume = UnityEngine.Random.Range(.95f, 1f);
    }

    public void ChangeBGM(AudioClip clip)
    {
        StartCoroutine(SwapBGM(clip));
    }

    IEnumerator SwapBGM(AudioClip clip){
        if (sourceBGM.clip.name == clip.name)
            yield return null;
        sourceBGM.volume = Mathf.MoveTowards(0, 1, .25f * Time.deltaTime);
        sourceBGM.Stop();
        sourceBGM.clip = clip;
        sourceBGM.volume = Mathf.MoveTowards(1, 0, .25f * Time.deltaTime);
        sourceBGM.Play();
        yield return null;
    }
    //Audio Mixer Controls
    public void SetMasterVolume(float volumeLevel)
    {
        aMixer.SetFloat("MasterVol", volumeLevel);
    }
    public void SetBGMVolume(float volumeLevel)
    {
        aMixer.SetFloat("BackgroundMusicVol", volumeLevel);
    }
    public void SetSFXVolume(float volumeLevel)
    {
        aMixer.SetFloat("SFXVol", volumeLevel);
    }
    public void SetAmbientVolume(float volumeLevel)
    {
        aMixer.SetFloat("AmbientVol", volumeLevel);
    }
}
