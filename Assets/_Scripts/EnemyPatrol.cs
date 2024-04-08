using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : Enemy
{
    public Transform[] patrolPoints;
    public float waitTime = 3f;
    int currentPointIndex;
    bool deciding;

    void Start(){
        speed = 1.75f;
        minimumDistance = 2f;
    } 
    
    void Update(){
        if(transform.position != patrolPoints[currentPointIndex].position){
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
        } else {

            if(deciding == false){
                deciding = true;
                StartCoroutine(Decide());
            }
        }
    }

    IEnumerator Decide(){
        yield return new WaitForSeconds(waitTime);
        if(currentPointIndex + 1 < patrolPoints.Length) {
            currentPointIndex++;
        } else {
            currentPointIndex = 0;
        }
        deciding = false;
    }
}