using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berrd : MonoBehaviour
{
    public float length = 2f;
    float timer;
    bool hit;
    [Header("Visual Effects")]
    public GameObject puffVFX;
    public GameObject featherVFX;

    void Update()
    {
        if(timer > 0){
            timer -= Time.deltaTime;
        }else if(timer <= 0 && hit){
            timer = 0;
            DestroyBird();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        if(!hit){
            timer = length;
            hit = true;
        }
        Instantiate(puffVFX, transform.position, transform.rotation);
        Instantiate(featherVFX, transform.position, transform.rotation);
    }
    
    void DestroyBird(){
        Destroy(gameObject);
        Instantiate(puffVFX, transform.position, transform.rotation);
        Instantiate(featherVFX, transform.position, transform.rotation);
    }
}
