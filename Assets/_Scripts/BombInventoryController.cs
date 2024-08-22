using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInventoryController : MonoBehaviour
{
    public Transform setpoint;
    public BombController[] bombTypes;
    int bombTypeID;
    void Start(){}

    public void SetBomb(){
        if (bombTypes.Length > 0){
            BombController preparedBomb = Instantiate (bombTypes[bombTypeID], setpoint.position, setpoint.rotation) as BombController;
        }
    }
    public void SetBombID(int newTypeID){
        bombTypeID = newTypeID;
    }
}
