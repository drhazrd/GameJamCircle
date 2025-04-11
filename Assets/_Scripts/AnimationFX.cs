using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimationFX : MonoBehaviour
{
    public Animator m_animator {get; private set;}
    BomberPlayerController bombPlayer;
    public bool alternate;
    public bool isSprinting;
    public bool isMoving;
    public bool isGrounded;
    public AudioClip footstepSFX;

    void Awake()
    {
        bombPlayer = GetComponentInParent<BomberPlayerController>();
        m_animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(bombPlayer != null){
            m_animator.SetBool("isGrounded", bombPlayer.isGrounded);
            m_animator.SetBool("Sprint", bombPlayer.isSprinting);
            m_animator.SetBool("isRunning", bombPlayer.isMoving);
        } else{
            m_animator.SetBool("isGrounded", isGrounded);
            m_animator.SetBool("Sprint", isSprinting);
            m_animator.SetBool("isRunning", isMoving);
        }

    }

    public void PlayFootStep(){
        if(footstepSFX != null) AudioManager.instance.PlaySFXClip(footstepSFX);
    }

    public void Action()
    {
        m_animator.SetTrigger("Action");
    }
    public void Jump()
    {
        m_animator.SetTrigger("Jump");
    }
    public void Hit()
    {
        m_animator.SetTrigger("Hit");
    }

    public void Interact()
    {
        if(alternate){
            m_animator.SetTrigger("Interact1");

        }else{
            m_animator.SetTrigger("Interact2");

        }
    }

    internal void Death()
    {
        m_animator.SetTrigger("Death");
    }

    public void Use()
    {
        m_animator.SetTrigger("Use");
    }
}
