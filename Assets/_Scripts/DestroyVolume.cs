using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DestroyVolume : MonoBehaviour
{
    Collider m_collider;

    void Awake(){
        m_collider = GetComponent<Collider>();
        m_collider.isTrigger = true;
    }
    void OnTriggerEnter(Collider col){
        Destroy(col.gameObject);
    }
}
