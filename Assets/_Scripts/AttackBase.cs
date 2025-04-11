using System;
using UnityEngine;
public abstract class AttackBase: MonoBehaviour {
    public LayerMask enemyLayer;
    public float attackPower = 10f;
    public GameObject hitVFX;

    public virtual void Attack(){
                        Debug.Log($"Attack");

    }
    public abstract void Setup(float newAttackPower);
}