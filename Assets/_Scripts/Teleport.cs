using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class Teleport: MonoBehaviour
{
    public bool playerHere{get; private set;}
    public float rangeDetected = .5f;
    SphereCollider _collider;
    public Transform t {get; private set;}

    void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = rangeDetected;
        _collider.isTrigger = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHere = true;
            t = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHere = false;
            t = null;
        }
    }
}

