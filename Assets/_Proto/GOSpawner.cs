using System.Collections;
using System;
using UnityEngine;

public class GOSpawner : MonoBehaviour
{
    public GameObject spawned;
    public float coolDown;
    public bool spawning;
    public bool singleSpawn;
    
    void Start(){
        if(!spawning){
            StartCoroutine(SpawnProcess());
        }
    }
    IEnumerator SpawnProcess(){
        spawning = true;
        GameObject obj = Instantiate(spawned, transform.position, transform.rotation);
        int typeCount = Enum.GetValues(typeof(PetType)).Length;
        yield return new WaitForSeconds(coolDown);
        spawning = false;
        Debug.Log("Spawn");
    }
}