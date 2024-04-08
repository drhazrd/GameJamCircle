using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    public float visionDistance;
    void Update(){
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, visionDistance);
        if(hitInfo.collider != null){
            // Do Some after hit thing 
        } else {
            // Do some thing else
        }
    }
}