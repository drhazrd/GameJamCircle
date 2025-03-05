using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombInventoryController : MonoBehaviour
{
    public Transform setpoint;
    public BombController[] bombTypes;
    public BombClass[] bombClasses;
    public BombController bombEquipped {get; private set;}
    public int bombTypeID {get; private set;} 
    public int bombClassID {get; private set;}
    public int bombDetonatorID {get; private set;}
    int bombStock, maxBombStock = 5;
    public bool stackOppurtunity;
    BombController stackTarget;
    DetonatorType detonator;
    bool detonatorReady;
    List <BombController> listOfRemoteDetonation = new List<BombController>();
    List <BombController> listOfLinkDetonation = new List<BombController>();
    private float linkDetonateDelay = 1f;

    void OnEnable()
    {
        BomberPlayerController.onDetonateAllBombs += ClearList;
        BombController.onBombDestroy += ClearStack;
    }
    void OnDisable()
    {
        BomberPlayerController.onDetonateAllBombs -= ClearList;
        BombController.onBombDestroy -= ClearStack;
    }

    void Start(){
        bombEquipped = bombTypes[bombTypeID];
        bombStock = maxBombStock;
    }

    public void SetBomb(){
        bool canSet = bombTypes.Length > 0 && bombStock > 0;
        if (canSet){
            if(stackOppurtunity){
                if(stackTarget != null){
                    stackTarget.Stack();
                } else SetStackTarget(null);
            } else {
                BombController preparedBomb = Instantiate (bombEquipped, setpoint.position, setpoint.rotation) as BombController;
                preparedBomb.delay = 5f;
                switch(bombDetonatorID){
                    case 0: // Assuming Fuse is 0
                        detonator = DetonatorType.Fuse;
                        break;
                    case 1: // Assuming Timer is 1
                        detonator = DetonatorType.Remote;
                        listOfRemoteDetonation.Add(preparedBomb);
                        break;
                    case 2: // Assuming Remote is 2
                        detonator = DetonatorType.Link;
                        listOfLinkDetonation.Add(preparedBomb);
                        break;
                    case 3: // Assuming Link is 3
                        detonator = DetonatorType.Proximity;
                        break;
                    default:
                        // Handle invalid IDs (e.g., return a default value or throw an exception)
                        detonator = DetonatorType.Fuse;
                        break;
                }
                preparedBomb.SetupBomb(detonator, BombType.Defensive);
            }
            bombStock--;
        } else {
            bombStock = maxBombStock;
            return;
        }
    }

    public void SetBombID(int id){
        if(id < 0){
            id = bombTypes.Length - 1;
        } else if (id > bombTypes.Length - 1){
            id = 0;
        }
        bombTypeID = id;
        bombEquipped = bombTypes[bombTypeID];
    }
    public void SetDetonatorTypeID(int id){
        if(id < 0){
            id = 3;
        } else if (id > 3){
            id = 0;
        }
        bombDetonatorID = id;
    }
    public void SetBombTypeID(int id){
        bombTypeID = id;
    }
    void SetBombClass(int id){
        bombClassID = id;
    }
    void SetDetonatorType(int detonatorID){
        bombDetonatorID = detonatorID;
    }
    void SetBombType(int typeID){
        bombTypeID = typeID;
    }
    public void SetStackTarget(BombController target){        
        stackOppurtunity = target != null ? true : false;
        stackTarget = target;
        
    }

    void Update()
    {
        detonatorReady = listOfRemoteDetonation.Count > 0 ? true : false; 
        BombCopUIManager.ui.UpdateUIData(bombClassID, bombDetonatorID, bombTypeID, bombStock, detonatorReady);
    }
    void ClearList(){
        listOfRemoteDetonation.Clear();
        
        if(listOfLinkDetonation.Count > 0){
            StartCoroutine(ClearLink());
        }
    }

    IEnumerator ClearLink()
    {
        for (int i = 0; i < listOfLinkDetonation.Count; i++)
        {
            listOfLinkDetonation[i].LinkDetonate();
            yield return new WaitForSeconds(linkDetonateDelay);
        }
        listOfLinkDetonation.Clear();
    }

    public void ClearStack(BombController bomb){
        Debug.Log("Clear Stack!");
        if(stackTarget == bomb) SetStackTarget(null);
    }
}

public class BombClass{
    public string className;
    public BombController bombObj;
    public int currentCount, maxCount;
}
public enum BombType{
    Tactical,
    Defensive,
    Powerful,
    Unpredictable,
    Wacky,
}
/* Tactical: Napalm, Shadow, Shrapnel, Cluster, Link, Logic, Time, Smart.
Defensive: Shield, Crest, Defender, Snake, Leash, Glowing.
Powerful: Ogre, Giant, Smash, Man, Cold/Freeze, Magic, Phlebotinum, Laser.
Unpredictable: Dirty, Stink, Rush, Big, Emotional, Bio, Neutron, Trick.
Wacky: Drama Bomb (distracts enemies), Incredibly Obvious Bomb (enemies ignore it), Smile Bomb (explodes into confetti).Tactical: Napalm, Shadow, Shrapnel, Cluster, Link, Logic, Time, Smart.
*/
