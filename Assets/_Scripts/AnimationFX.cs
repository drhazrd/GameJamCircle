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
    public bool isSprinting;
    public bool isMoving;

    void Awake()
    {
        bombPlayer = GetComponentInParent<BomberPlayerController>();
        m_animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(bombPlayer != null){
            m_animator.SetBool("Sprint", bombPlayer.isSprinting);
            m_animator.SetBool("isRunning", bombPlayer.isMoving);
        } else{
            m_animator.SetBool("Sprint", isSprinting);
            m_animator.SetBool("isRunning", isMoving);
        }

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
