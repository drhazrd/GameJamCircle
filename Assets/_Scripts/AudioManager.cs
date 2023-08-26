using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
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
    internal void StartLevelBGM()
    {
        currentBGM = levelBGM;
        if (currentBGM != null)
        {

            sourceBGM.Stop();
            sourceBGM.clip = levelBGM;
            sourceBGM.Play();
        }
    }

    void OnEnable() {
        GameManager.ongameStart += StartLevelBGM;
    }
    void OnDisable() {
        GameManager.ongameStart -= StartLevelBGM;        
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
    private void SoundRandomizer()
    {
        sourceSFX.pitch = UnityEngine.Random.Range(.75f, 1.25f);
        sourceSFX.volume = UnityEngine.Random.Range(.85f, 1f);
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
