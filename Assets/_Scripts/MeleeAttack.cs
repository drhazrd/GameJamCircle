using System;
using UnityEngine;
public class MeleeAttack: AttackBase {
    TestPlayer player;
    bool canAttack;
    public float range = 3f;
    void Start()
    {
        player = GetComponent<TestPlayer>();
        if(player != null){
            player.onActionPressed += Attack;
            canAttack = player != null;
        }
    }
    public override void Attack(){
        if(canAttack){
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyLayer);
            foreach(Collider nearbyObject in colliders){
                Rigidbody enemy = nearbyObject.GetComponent<Rigidbody>();
                if(enemy != null){
                    Vector3 meleeForce = nearbyObject.transform.position - transform.position;
                    meleeForce = meleeForce.normalized;
                    meleeForce = meleeForce * attackPower;
                    enemy.AddForce(meleeForce);
                    if(hitVFX) Instantiate(hitVFX, nearbyObject.transform.position, Quaternion.identity);
                }
            }
        }
    }
    public override void Setup(float newAttackPower){
        base.attackPower = newAttackPower;
    }
    void Oestroy()
    {
        if(player != null){
            player.onActionPressed -= Attack;
        }
    }
}