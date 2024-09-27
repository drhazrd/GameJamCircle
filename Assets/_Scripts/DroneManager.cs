using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DroneManager : MonoBehaviour
{
    public static DroneManager droneBase;
    public Transform[] PathPositions;
    public Transform[] PathParent;
    public int PathParentID {get; private set;}

    public GameObject drone;
    public List<Drone> drones = new List<Drone>();
    public Transform spawnTarget;
    public bool canSpawn;
    public float spawnDelay = .5f;
    [Range(3, 15)]
    public int droneSpeed = 5;

    [Header("Wave Data")]
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    int waveNumber = 0;
    public TextMeshProUGUI waveDataText;

    void Awake()
    {
        if (droneBase != null && DroneManager.droneBase != this){Destroy(this);}else if (DroneManager.droneBase == null){droneBase = this;}
        GetPath();
    }
    void Start(){
    }
    void Update(){
        if(canSpawn){
            if(countdown <= 0f){
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
            } else {
                countdown -= Time.deltaTime;
            }
        }
        UpdateUIData();
    }

    private void UpdateUIData()
    {
        float timer = Mathf.Round(countdown);
        if(waveDataText)waveDataText.text = $"Wave {waveNumber + 1} in {timer}...";
    }

    private IEnumerator SpawnWave()
    {
        waveNumber++;
        for(int i = 0; i < waveNumber; i++){
            SpawnNextDrone();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnNextDrone(){
            GameObject createdDrone = Instantiate(drone, spawnTarget.position, spawnTarget.rotation);
            Drone newDrone = createdDrone.GetComponent<Drone>();
            drones.Add(newDrone);
            int newDroneSpeed = droneSpeed * Random.Range(1,5);
            newDrone.SetPath(PathPositions, newDroneSpeed);
            GetPath();
    }

    void GetPath(){
        PathParentID = Random.Range(0, PathParent.Length);
        PathPositions = new Transform[PathParent[PathParentID].childCount];
        for(int i = 0; i < PathPositions.Length; i++){
            PathPositions[i] = PathParent[PathParentID].GetChild(i);
        }
    }
}
