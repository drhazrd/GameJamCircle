using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    public RoomType spawnDirection;

    public RoomTemplates templates;
    private int rand;
    
    public bool spawmed = false;
    public bool center;

    float waitTime = 1f;

    void Start(){
        Destroy(gameObject,waitTime);
        BoxCollider2D m_collider = GetComponent<BoxCollider2D>();
        m_collider.isTrigger = true;
        templates = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomTemplates>();
        Invoke("Spawn", .025f);
    }

    void Spawn()
    {
        if(!spawmed){
            switch(spawnDirection){
                case RoomType.Left:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                    break;
                case RoomType.Right:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                    break;
                case RoomType.Top:
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                    break;
                case RoomType.Bottom:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                    break;
            }
        }
            spawmed = true;
            templates.rooms.Add(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("SpawnPoint")){
            if(other.GetComponent<RoomSpawner>().spawmed == false && spawmed == false){
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Debug.Log(gameObject);
                spawmed = true;
            }
            templates.rooms.Remove(gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player")){
            //Destroy(gameObject);
        }
    }
}
public enum RoomType{
    Left,
    Right,
    Top,
    Bottom
}
