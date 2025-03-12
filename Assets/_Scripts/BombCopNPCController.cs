using System;
using UnityEngine;
using UnityEngine.AI;

public class BombCopNPCController : MonoBehaviour
{
    public Alliance alligenceBehavior;
    NavMeshAgent agent;
    AnimationFX anim;
    bool altInteract;
    private bool isGrounded, isMoving, isSprinting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<AnimationFX>();
        switch(alligenceBehavior){
            case Alliance.Friend:

            break;
            case Alliance.Neutral:

            break;
            case Alliance.Ally:

            break;
            case Alliance.Enemy:
            
            break;
            
            default:
            break;
        }
    }
    void Action(){
        anim.Action();
    }
    void Interact(){
        anim.Interact();
        anim.alternate = altInteract;
    }
    void Motion(){
        anim.isGrounded = isGrounded;
        anim.isMoving = isMoving;
        anim.isSprinting = isSprinting;
    }
}

public enum Alliance{
    Friend,
    Neutral,
    Ally,
    Enemy
}