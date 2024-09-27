using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{  
    public static RoomTemplates templateManager; 
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    
    public GameObject closedRoom;

    public List <GameObject> rooms = new List <GameObject>();

    public float waitTime = 5f;
    [SerializeField]private bool debug = false;
    private bool spawnedExit = false;
    private bool spawnedBoss = false;
    private bool spawnedTreasure = false;
    private int spawnedTreasureCount = 0;
    private int maxSpawnedTreasureCount;
    public Transform bossLocation{get; private set;}
    public GameObject exit;
    public GameObject boss;
    public GameObject treasure;
    public GameObject player;
    
    void Start(){
        if(RoomTemplates.templateManager == null&& templateManager == null){
            templateManager = this;
        } else {
            Destroy(this);
        }
        maxSpawnedTreasureCount = Random.Range(3,5);
    }
    void Update(){
        WaitTimer();
    }
    
    void SpawnBoss(){
        if(waitTime <= 0 && !spawnedBoss){           
            int bid = rooms.Count - 1;
            bossLocation = rooms[bid].transform;
            Instantiate(boss, bossLocation.position, bossLocation.rotation);
            Debug.Log("Set Boss Spawn");
            spawnedBoss = true;
        } 
    }
    

    
    void SpawnTreasure(){
        if(waitTime + .5f <= 0 && spawnedTreasure == false){
            for (int i = 0; i < maxSpawnedTreasureCount; i++){
                int randomSpawned = Random.Range(3, rooms.Count - 2);
                Instantiate(treasure, rooms[randomSpawned].transform.position, rooms[randomSpawned].transform.rotation);
                spawnedTreasureCount++;
                Debug.Log("Treasure");
            }
            spawnedTreasure = true;
        }

    }
    void WaitTimer(){
        if(waitTime > 0) waitTime -= Time.deltaTime; else waitTime = 0; SpawnLevelAssets(); return;
    } 
    void SpawnLevelAssets(){
        SpawnBoss();
        SpawnTreasure();
        SpawnExit();
    }
    void SpawnExit(){
        if(waitTime + .25f <= 0 && !spawnedExit){           
            int bid = rooms.Count - 1;
            bossLocation = rooms[bid].transform;
            Instantiate(boss, bossLocation.position, bossLocation.rotation);
            Debug.Log("Set Boss Spawn");
            spawnedBoss = true;
        } 
    }
}
