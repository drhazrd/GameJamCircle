using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawned;
    public float coolDown;
    public bool spawning;
    public bool singleSpawn;
    
    void Update(){
        //if(!spawning){ StartCoroutine(SpawnProcess());}else 
        if(!spawning){
            StartCoroutine(SpawnProcess());
        }
    }
    IEnumerator SpawnProcess(){
        coolDown -= .3f;
        spawning = true;
        GameObject obj = Instantiate(spawned, transform.position, transform.rotation);
        obj.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        yield return new WaitForSeconds(coolDown);
        spawning = false;
        Debug.Log("Spawn");
    }
    
}
