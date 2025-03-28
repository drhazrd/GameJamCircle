using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombCopNPCController : MonoBehaviour
{
    public Alliance alligenceBehavior;
    public NavMeshAgent agent{get; private set;}
    public float stateTimeElapsed { get; private set; }

    AnimationFX anim;
    bool altInteract;
    private bool isGrounded, isMoving, isSprinting;

    [Header("Navigation")]
    public List<Transform> waypointList;
    public int nextWaypoint;
    public Transform eyes;
    bool aiActive;
    public State currentState;
    public State remainState;
    public Transform chaseTarget;
    
    [Header("Stats")]
    public float lookRadius = 1.5f;
    public float lookRange = .75f;
    public float attackRange = .5f;
    public float attackForce = 1f;
    public float attackRate = .5f;
    public float searchTurnSpeed = 2f;
    public float searchDuration = 3f;

    private AttackController attack;
    public GameObject attackObject;
    public Transform actionPoint;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<AnimationFX>();
        attack = GetComponent<AttackController>();
        if (attack){
            attack.AttackSetup(attackObject, actionPoint);
        }
    }

    void Start()
    {
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
    public void Attack(){
        //Attack
        Action();
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
    public void SetupAI(bool aiActivation, List<Transform>managerWaypoints){
        aiActive = aiActivation;
        waypointList = managerWaypoints;
        if(aiActive){
            agent.enabled = true;
        } else 
            agent.enabled = false;
    }
    void Update()
    {
        if(!aiActive){
            return;
        }
        currentState.UpdateState(this);
    }
    void OnDrawGizmos()
    {
        if(currentState != null && eyes != null){
            Gizmos.color = currentState.gizmoColor;
            Gizmos.DrawWireSphere(eyes.position, lookRadius);
        }
    }
    public void TransitionToState(State nextState){
        if(nextState != remainState){
            currentState = nextState;
        }
    }
    public bool CheckIfCountdownElapsed(float duration){
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    void OnExitState(){
        stateTimeElapsed = 0;
    }

    public void Attack(float attackForce, float attackRate)
    {
        if(attack != null){
            attack.PerformAttack();
        }
    }
}



public enum Alliance{
    Friend,
    Neutral,
    Ally,
    Enemy
}