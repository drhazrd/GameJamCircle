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

    float healthCurrent;
    public float healthMax;
    public SliderBar statusBar;
    StrategyManager manager;


    void Start(){
        if(spawnArea == null){
            spawnArea = this.transform;
        }
        if(statusBar != null){
            statusBar.SetupBar(healthCurrent, healthMax, this.name);
        }
        StartCoroutine(MobSpawnProcess(timeBetweenSpawn));
        manager = StrategyManager.gameAdjudicator;
        mainBase = manager.mainBaseLocation;
        int resourceIndex = Random.Range(0, manager.resourceLocations.Count - 1);
        resource = manager.resourceLocations[resourceIndex].transform;
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
    void Update(){
        if(healthCurrent > 0){
            statusBar.UpdateBar(healthCurrent);
        }
    }
}


public enum BuildingType{
    Resource,
    Harvest,
    Combat, 
    Defense
}