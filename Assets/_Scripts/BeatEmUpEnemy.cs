using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEmUpEnemy : MonoBehaviour
{
    public float speed = 1f;
    public float chaseDistance = 2.5f;
    public float stopDistance = .5f;
    public GameObject target;

    Animator animator;

    private float targetDistance;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if(targetDistance < chaseDistance && targetDistance > stopDistance){
            ChasePlayer();
        } else {
            StopChasePlayer();
        }    
    }

    void ChasePlayer(){
        if(transform.position.x < target.transform.position.x){
            GetComponent<SpriteRenderer>().flipX = false;
        } else {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        animator.SetBool("isWalking", true);
    }

    void StopChasePlayer(){
        animator.SetBool("isWalking", false);
        ///...
    }
}
