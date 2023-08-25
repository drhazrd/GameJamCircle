using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    Animator anim;
    private float nextTimeToFire = 0f;
    [SerializeField] AudioClip fireSFX, reloadSFX, emptysfx;

    void Start(){
        anim = GetComponent<Animator>();
    }
    void Update(){
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    void Shoot(){
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        if(fireSFX != null) AudioManager.instance.PlaySFXClip(fireSFX);
        anim.SetTrigger("Fire");
    }
}