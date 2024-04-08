using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHoming : MonoBehaviour
{
    Vector3 targetPosition;
    public float speed;

    void Start(){
        //targetPosition = FindObjectOfType<PlayerController>().transform.position;
    }

    void Update(){
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition){
            Destroy(gameObject);
        }
    }
}