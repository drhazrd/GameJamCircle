using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 200f;
    public float lifeTime = 4.0f;
    public AudioSource fireSound;
    // Start is called before the first frame update
    void Awake()
    {
        
        rb.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Destroy(gameObject, lifeTime);
        if(fireSound != null)
            fireSound.Play();
    }

}
