using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [HideInInspector] public int damage = 1;
    BoxCollider2D m_collider;

    void Start(){
        m_collider = GetComponent<BoxCollider2D>();
        m_collider.isTrigger = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag){
            case "Wall":
            Destroy(this.gameObject);
            break;
            
            case "Enemy":
            Destroy(this.gameObject);
            other.gameObject.GetComponent<Damageable>().TakeDamage(damage);
            break;
            
            case "Target":
            Destroy(this.gameObject);
            Destroy(gameObject);
            break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}