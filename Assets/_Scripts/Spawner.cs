using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawned;
    public float coolDown;
    public bool spawning;
    public bool singleSpawn;
    
    void Start(){
        //if(!spawning){ StartCoroutine(SpawnProcess());}else 
        if(singleSpawn){
            StartCoroutine(SpawnProcess());
        }
    }
    IEnumerator SpawnProcess(){
        spawning = true;
        GameObject obj = Instantiate(spawned, transform.position, transform.rotation);
        yield return new WaitForSeconds(coolDown);
        spawning = false;
    }
    
}
