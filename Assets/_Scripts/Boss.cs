using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform currentPlayer;
    [SerializeField] Transform firePoint;
    public LayerMask ground, player;
    
    [SerializeField] int health = 10;
    public GameObject projectile;
    public AudioClip fireSFX;

    //Attacking
    public float timeBetweenAttacks = .5f;
    bool alreadyAttacked;

    //Attacking
    public float sightRange = 20f, attackRange = 12f, findRange = 17f;
    public bool inSightRange, inAttackRange, isInRange;

    void Start()
    {
        currentPlayer = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, sightRange, player);
        isInRange = Physics.CheckSphere(transform.position, findRange, player);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, player);

        if(!inSightRange && !inAttackRange)Patrol();
        if(inSightRange && !inAttackRange)Find();
        if(inSightRange && inAttackRange)Attack();
    }
    void Patrol()
    {
       transform.Rotate(0, 15f * Time.deltaTime ,0);
    }
    void Find()
    {
        Quaternion targetRotation = Quaternion.LookRotation(currentPlayer.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);    }
    void Attack()
    {        
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
        if(fireSFX != null) AudioManager.instance.PlaySFXClip(fireSFX);
    }
}
