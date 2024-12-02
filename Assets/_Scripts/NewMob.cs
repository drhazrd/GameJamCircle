using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewMob : MonoBehaviour
{
    public float descisionTime;
    public float speed = 1f;
    public float harvestTime = 1f;
    public float harvestAmount = 1f;
    public float depositTime = 1f;
    public Transform targetBase;
    public Transform targetResource;
    Transform currentTarget;
    bool canAct;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        canAct = true;
        currentTarget = targetResource;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetResource == null)
        {
            MoveToLocation(targetBase.position);
        }
        else
        {
            if (canAct)
            {
                MoveToLocation(currentTarget.position);
                transform.LookAt(currentTarget);
            }
        }
    }

    public void SpawnInfo(float h, float d, float s)
    {
        harvestTime = h;
        depositTime = d;
        speed = s;
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = speed;
        }
    }

    public void UpdateTarget(Transform t1, Transform t2)
    {
        targetBase = t1;
        targetResource = t2;
        currentTarget = targetResource;
    }

    private void MoveToLocation(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        if (Vector3.Distance(transform.position, position) < 0.1f)
        {
            canAct = false;
            StartCoroutine(performAction());
        }
    }

    private IEnumerator performAction()
    {
        yield return new WaitForSeconds(descisionTime);
        switchTarget();
        canAct = true;
    }

    private void switchTarget()
    {
        if (currentTarget == targetResource)
        {
            currentTarget = targetBase;
        }
        else
        {
            currentTarget = targetResource;
        }
    }
}
