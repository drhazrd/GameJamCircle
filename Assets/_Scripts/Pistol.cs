using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    Animator anim;
    private float nextTimeToFire = 0f;
    [SerializeField] AudioClip fireSFX, reloadSFX, emptysfx;
    InputManager controls;

    void Start(){
        isReloading = false;
        currentAmmo = maxAmmo;
        anim = GetComponent<Animator>();
        controls = GetComponentInParent<InputManager>();
    }
    void OnEnable(){
        isReloading = false;
    }
    void Update(){
        if (isReloading) return;
        if (currentAmmo <= 0){
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    IEnumerator Reload(){
        isReloading = true;
        yield return new WaitForSeconds(reloadTime - .25f);
        if(reloadSFX != null) AudioManager.instance.PlaySFXClip(reloadSFX);
        currentAmmo = maxAmmo;
        yield return new WaitForSeconds(.25f);
        isReloading = false;
    }
    void Shoot(){
        currentAmmo--;
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        if(fireSFX != null) AudioManager.instance.PlaySFXClip(fireSFX);
        if(anim!=null)anim.SetTrigger("Fire");
        CameraEffects.camEffects.Shake(.05f);
    }
}