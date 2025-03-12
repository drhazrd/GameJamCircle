using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health{get; private set;}
    public int maxHealth = 3;
    public GameObject deathVFX;
    public AudioClip deathSFX;
    private float deathDelay = .3f;
    private AnimationFX anim;
    public bool isImmortal{get; private set;}
    [SerializeField]
    bool immortalOverride;

    void Awake()
    {
        anim = GetComponentInChildren<AnimationFX>();
        if(immortalOverride){
            Immortalize();
        }
        health = maxHealth;
    } 

    public virtual void TakeDamage(int damage){

        if(!isImmortal){
            health -= damage;
            if(health <= 0){
                Die();
            }
        }
        if(anim != null) anim.Hit();
    }
    public virtual void Die(){
        if(anim != null){
            anim.Death();
            deathDelay = anim.m_animator.playbackTime + 5f;
        }
        if(deathSFX != null) AudioManager.instance.PlaySFXClip(deathSFX);
        if(deathVFX != null) Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(gameObject, deathDelay);
    }

    public void Immortalize(){
        isImmortal = true;
    }
    public void Remortalize(){
        isImmortal = false;
    }
}
