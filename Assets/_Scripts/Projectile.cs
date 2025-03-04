using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [HideInInspector] public int damage = 1;
    float lifeTime;
    BoxCollider2D m_collider;

    void Start(){
        m_collider = GetComponent<BoxCollider2D>();
        m_collider.isTrigger = false;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        switch(other.gameObject.tag){
            case "Wall":
            break;
            
            case "Enemy":
            other.gameObject.GetComponent<Damageable>().TakeDamage(damage);
            break;
            
            case "Target":
            break;

        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if(lifeTime >= 2f){
            Destroy(gameObject);
        }
    }
    
}