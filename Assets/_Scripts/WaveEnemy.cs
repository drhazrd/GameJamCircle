using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemy : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
            //GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnTotal++;
        }
     
    }
}
