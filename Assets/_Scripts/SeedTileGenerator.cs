using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTileGenerator : MonoBehaviour
{
    public TileSpawner[] spawners;
    public List<Transform> spawnlocations = new List<Transform>();
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Setup()
    {
        Instantiate(spawners[0].gameObject, spawnlocations[0].transform.position, spawnlocations[0].transform.rotation);
    }
}
