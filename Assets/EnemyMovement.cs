using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] Transform currentPlayer;
    [SerializeField] Transform firePoint;
    public LayerMask ground, player;
    
    [SerializeField] int health = 10;
    public GameObject projectile;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 5f;

    //Attacking
    public float timeBetweenAttacks = .5f;
    bool alreadyAttacked;

    //Attacking
    public float sightRange = 20f, attackRange = 12f, chaseRange = 17f;
    public bool inSightRange, inAttackRange, inChaseRange;


    void Awake()
    {
        currentPlayer = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();        
    }

    void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, sightRange, player);
        inChaseRange = Physics.CheckSphere(transform.position, chaseRange, player);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, player);

        if(!inSightRange && !inAttackRange)Patrol();
        if(inSightRange && !inAttackRange)Chase();
        if(inSightRange && inAttackRange)Attack();
    }
    void Patrol()
    {
        if(!walkPointSet) SearchWalkPoint();
        if(walkPointSet){
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }
    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, ground)){
            walkPointSet = true;
        }
    }
    void Chase()
    {
        agent.SetDestination(currentPlayer.position);
    }
    void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(currentPlayer);
        
        if(!alreadyAttacked){
            Shoot();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack(){
        alreadyAttacked = false;
    }

    void Shoot(){
        Rigidbody rb = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            Invoke(nameof(Die), 2f);
        }
        StartCoroutine(Hurt());
    }
    IEnumerator Hurt(){
        Material currentMaterial = GetComponentInChildren<MeshRenderer>().materials[0];
        Color currentColor = currentMaterial.color;
        currentMaterial.color = Color.white;
        yield return new WaitForSeconds(1f);
        currentMaterial.color = currentColor;
    }
    public void Die(){
        Destroy(gameObject);
    }
    void OnDrawGizmoSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
