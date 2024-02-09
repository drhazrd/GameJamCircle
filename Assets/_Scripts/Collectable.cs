using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (SphereCollider))]
public class Collectable : MonoBehaviour
{
    SphereCollider m_col;
    public int xpCost = 10;
    public GameObject collectEffect;

    void Start(){
        m_col = GetComponent<SphereCollider>();
        m_col.isTrigger = true;
    }

    void OnTriggerEnter (Collider col){
        if (col.tag == "Player"){
            Destroy(gameObject);
            PlayerStats.stats.SetExperience(xpCost);
            if(collectEffect != null) Instantiate(collectEffect, transform.position, transform.rotation);
        }
    }
}
