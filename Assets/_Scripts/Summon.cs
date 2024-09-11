using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    List<Transform> navigationPoints = new List<Transform>();
    //Transform[] pathTargets;
    int targetIndex = 0;
    Transform currentTarget;
    public Transform nextTarget;
    SummonManager sManager;
    int speed;
    public GameObject destroyVFX;


    public void SetPath(Transform[] path, int s){
        navigationPoints.AddRange(path);
        nextTarget = navigationPoints[targetIndex];
        speed = s;
    }
    
    void Start(){
        sManager = SummonManager.summons;
    }
    
    public void Update(){
        if(navigationPoints != null){
            Vector3 dir = nextTarget.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, nextTarget.position) <= 0.2f){
                GetNextPathPoint();
            }
        }
    }
    
    void GetNextPathPoint(){
        if(targetIndex > navigationPoints.Count - 1){
            sManager.activeSummons.Remove(this);
            DestroyDrone();
        }else {
            targetIndex++;
            nextTarget = navigationPoints[targetIndex];
        }
    }
    
    void DestroyDrone(){
            if(destroyVFX != null) Instantiate(destroyVFX, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
    }
}
