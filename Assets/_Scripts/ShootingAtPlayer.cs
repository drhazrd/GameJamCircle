using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAtPlayer : MonoBehaviour
{
    public GameObject bullet;
    public GameObject target;
    public float fireTimer = 0f;
    public float fireRate = 4f;
    public AudioClip fireSound;
    public AudioClip reloadSound;

    
    void Update()
    {
        
        fireTimer += Time.deltaTime;
        if(fireTimer >= fireRate)
        {
            fireTimer -= fireRate;
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        if(bullet != null){
            if(fireSound != null)
                AudioManager.instance.PlayClip(fireSound);
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(.5f);
            if(reloadSound != null)
                AudioManager.instance.PlayClip(reloadSound);
            StartCoroutine(gameObject.GetComponent<CameraControls>().ResetPosition());
        }
    }
}
