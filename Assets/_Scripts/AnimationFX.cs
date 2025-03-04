using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimationFX : MonoBehaviour
{
    Animator m_animator;
    BomberPlayerController bombPlayer;
    public bool alternate;

    void Awake()
    {
        bombPlayer = GetComponentInParent<BomberPlayerController>();
        m_animator = GetComponent<Animator>();
    }
    void Update()
    {
        m_animator.SetBool("Sprint", bombPlayer.isSprinting);
        m_animator.SetBool("isRunning", bombPlayer.isMoving);

    }

    public void PlayFootStep(){
        //AudioManager.PlayClip();
    }

    public void Action()
    {
        m_animator.SetTrigger("Action");
    }

    public void Interact()
    {
        if(alternate){
            m_animator.SetTrigger("Interact1");

        }else{
            m_animator.SetTrigger("Interact2");

        }

    }
}
