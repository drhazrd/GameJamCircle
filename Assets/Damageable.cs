using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int health = 3;
    public GameObject deathVFX;
    public AudioClip deathSFX;


    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            Instantiate(deathVFX, transform.position, transform.rotation);
            Die();
        }
    }
    void Die(){
        AudioManager.instance.PlaySFXClip(deathSFX);
        if(deathSFX != null){
        }
        Destroy(gameObject);
    }
}
