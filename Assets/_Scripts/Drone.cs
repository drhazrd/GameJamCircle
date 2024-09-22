using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    List<Transform> navigationPoints = new List<Transform>();
    Transform[] pathTargets;
    int targetIndex = 0;
    Vector3 velocity;
    NavMeshAgent agent_motor;
    Transform currentTarget;
    public Transform nextTarget;
    DroneManager dManager;
    public Transform model;

    int speed;
    public GameObject destroyVFX;


    public void SetPath(Transform[] path, int s){
        pathTargets = path;
        nextTarget = pathTargets[targetIndex];
        speed = s;
    }
    void Start(){
        dManager = DroneManager.droneBase;
    }
    public void Update(){
        if(pathTargets != null){
            Vector3 dir = nextTarget.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, nextTarget.position) <= 0.2f){
                GetNextPathPoint();
            }
        }
        if(model != null){
            Vector3 lookDirection = new Vector3(nextTarget.position.x, transform.localPosition.y, nextTarget.position.z);
            model.LookAt(lookDirection);
        }
    }
    void GetNextPathPoint(){
        if(targetIndex > pathTargets.Length - 1){
            dManager.drones.Remove(this);
            DestroyDrone();
        }else {
            targetIndex++;
            nextTarget = pathTargets[targetIndex];
        }
    }
       void DestroyDrone(){
            if(destroyVFX != null) Instantiate(destroyVFX, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
        }
}
