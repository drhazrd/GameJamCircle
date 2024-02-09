using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public List<GameObject> tiles = new List<GameObject>();
    BoxCollider2D m_collider;

    void Start()
    {
        m_collider = gameObject.AddComponent<BoxCollider2D>();
        m_collider.isTrigger = true;
        int tileID = Random.Range(0, tiles.Count);
        Instantiate(tiles[tileID], transform.position, transform.rotation);
    }
}
