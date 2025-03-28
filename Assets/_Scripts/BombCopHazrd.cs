using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class BombCopHazrd:MonoBehaviour
{
    Collider localCollider;
    float cooldown = 2f;
    public int damage = 1;
    public bool oneShot;
    bool canDamage;
    CinemachineImpulseSource impluse;
    public AttackType type;
    float newForce = .2f;

    void Start()
    {
        impluse = GetComponent<CinemachineImpulseSource>();
        localCollider = GetComponent<Collider>();
        localCollider.isTrigger = true;
        canDamage = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && oneShot){
            if(other.TryGetComponent<BombCopHealth>( out BombCopHealth player)){
                HurtBombPlayer(player);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && canDamage){
            if(other.TryGetComponent<BombCopHealth>( out BombCopHealth player)){
                HurtBombPlayer(player);
            }
        }
    }

    private void HurtBombPlayer(BombCopHealth playerHealth)
    {
        playerHealth.TakeDamage(damage);
        StartCoroutine(DamageCoolDown());
        impluse.GenerateImpulse(newForce);
        if(oneShot)Destroy(gameObject);

    }
    IEnumerator DamageCoolDown(){
        canDamage = false;
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }
}
