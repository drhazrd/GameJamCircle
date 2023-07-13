using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawned;

    void OnEnable(){
        GameManager.onGameStart += SpawnPlayer;
    }
    void OnDisable(){
        GameManager.onGameStart -= SpawnPlayer;
    }
    void SpawnPlayer(){
        Instantiate(spawned, transform.position, transform.rotation);
    }
}
