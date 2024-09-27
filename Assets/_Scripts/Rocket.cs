using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform target;
    public float speed = 3f;
    public GameObject destroyVFX;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame){
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        Destroy(gameObject);
        if(destroyVFX != null) Instantiate(destroyVFX, transform.position, transform.rotation);
    }
}
