using UnityEngine;
using System;
public class Spawner2D: MonoBehaviour{
    public GameObject spawnPrefab;

    public int spawnRate = 2;
    [Range(.001f, 1f)]
    public float timeBetweenSpwans;
    public float timeBetweenDespwans = 3f;
    float timer;

    void Update(){
        timer += Time.deltaTime;

        if(timer >= timeBetweenSpwans){
            timer = 0;
            SpawnObject();
        }
    }
    void SpawnObject(){
        for(int i = 0; i < spawnRate; i++){
            GameObject newSpawn = Instantiate(spawnPrefab, transform.position, transform.rotation) as GameObject;
            Destroy(newSpawn, timeBetweenDespwans);
        }
    } 


}