using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    RoomTemplates manager;
    void Start(){
        manager = RoomTemplates.templateManager;
        manager.rooms.Add(gameObject);
    }
    void OnDestroy(){
        if(manager != null) manager.rooms.Remove(gameObject);
    }
}
