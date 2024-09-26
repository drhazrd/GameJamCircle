using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobUnit : MonoBehaviour
{
    float decisionTimer;
    int level = 1;
    public float speed = 1f;
    public float harvestTime = 3f;
    public float depositTime = 1.5f;
    public Transform targetBase;
    public Transform targetResource;
    Transform currentTarget;
    bool canAct;

    void Start()
    {
        // Initialize the mob unit
        canAct = true;
        currentTarget = targetResource; // Start by moving to the resource
    }

    void Update()
    {
        if (canAct)
        {
            MoveToLocation(currentTarget.position);
        }
    }

    public void SpawnInfo(float ht, float dt, float s){
        harvestTime = ht;
        depositTime = dt;
        speed = s;
    }

    public void UpdateTargets(Transform t1, Transform t2){
        targetBase = t1;
        targetResource = t2;
        currentTarget = targetResource;
    }
    void SwitchTarget(){
        //Switch the target by setting timer harvestTime and the resource loctation or sets the timer to the base location and the depositTime on the timer
        if (currentTarget == targetResource)
        {
            currentTarget = targetBase;
            decisionTimer = depositTime;
        }
        else
        {
            currentTarget = targetResource;
            decisionTimer = harvestTime;
        }
    }
    void MoveToLocation(Vector3 pos){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pos, step);

        if (Vector3.Distance(transform.position, pos) < 0.1f)
        {
            canAct = false;
            StartCoroutine(PerformAction());
        }
    }

    IEnumerator PerformAction()
    {
        yield return new WaitForSeconds(decisionTimer);
        SwitchTarget();
        canAct = true;
    }
}
