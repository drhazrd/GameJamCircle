using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform spawnpoint;
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player")){
            GameObject obj = col.transform.gameObject;
            obj.GetComponent<PlayerHealth>().ChangeHealth(-25f);
            obj.transform.position = spawnpoint.position;
            Debug.Log("Respawn");
        }                
    }
}
