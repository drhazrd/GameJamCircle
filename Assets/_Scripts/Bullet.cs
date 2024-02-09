using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    [SerializeField] float lifeTime = 2.3f;
	public int damageToGive = 1;
	public GameObject impactEffect;
    
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        
        if(lifeTime <= 0){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
		{
			other.gameObject.GetComponent<Damageable>().TakeDamage(damageToGive);
		}
		
		if(impactEffect != null) Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
