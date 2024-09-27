using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int cost;
    public int value;
    public float timeToBuild;
    public BuildingType type;
    Transform spawnArea;
    public MobUnit mobSpawn;
    float timeBetweenSpawn = 2.5f;
    public bool autoSpawn;
    public bool canSpawn;
    bool spawnReady;
    Transform resource;
    Transform mainBase;
    int currentSpawn;
    public int maxSpawn = 5;

    void Start(){
        if(spawnArea == null){
            spawnArea = this.transform;
        }
        StartCoroutine(MobSpawnProcess(timeBetweenSpawn));
        mainBase = StrategyManager.gameAdjudicator.mainBaseLocation;
        resource = StrategyManager.gameAdjudicator.resourceLocations[0].transform;
    }

    public void SpawnMob(){
        autoSpawn = currentSpawn < maxSpawn;
        if(mobSpawn != null && !spawnReady){
            MobUnit newUnit = Instantiate(mobSpawn, spawnArea.position, spawnArea.rotation) as MobUnit;
            newUnit.UpdateTargets(transform, resource);
            spawnReady = true;
        }

    }

    IEnumerator MobSpawnProcess(float time){
        while(autoSpawn && canSpawn){
            yield return new WaitForSeconds(time);
            SpawnMob();
            currentSpawn++;
            spawnReady = false;
        }
    }

}
public enum BuildingType{
    Resource,
    Harvest,
    Combat, 
    Defense
}