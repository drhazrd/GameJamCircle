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
            Die();
        }
    }
    void Die(){
        if(deathSFX != null) AudioManager.instance.PlaySFXClip(deathSFX);
        if(deathVFX != null) Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(gameObject, 1f);
    }
}
