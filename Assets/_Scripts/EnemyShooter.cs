using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    public GameObject projectile;
    public float timeBetweenShots;
    private float nextShotTime;

    void Start(){
        speed = 1.75f;
        minimumDistance = 2f;
    } 
    
    void Update(){
        if(Time.time > nextShotTime){

        }
        if(Vector2.Distance(transform.position, target.position) > minimumDistance){
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else {
            //Attack Code Here
        }
    }
}