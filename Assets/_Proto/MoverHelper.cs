using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MoverHelper : MonoBehaviour{
    GameObject player;
    BoxCollider m_collider;
    
    void Start(){
        m_collider = GetComponent<BoxCollider>();
        m_collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            player = col.gameObject;
            player.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            player.transform.SetParent(null);
            player = null;
        }
    }
}
