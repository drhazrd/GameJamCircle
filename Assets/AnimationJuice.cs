using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationJuice : MonoBehaviour
{
    public AudioClip footStep;

    public void PlayFootStep(){
        if(footStep != null) AudioManager.instance.PlaySFXClip(footStep);
    }
}
