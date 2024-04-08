using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : Enemy
{

    void Start(){
        speed = 1.75f;
        minimumDistance = 2f;
    } 
    
    void Update(){
        if(Vector2.Distance(transform.position, target.position) > minimumDistance){
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else {
            //Attack Code Here
        }
    }
}