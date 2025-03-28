using System;
using UnityEngine;
public class AttackController: MonoBehaviour
{
    public AttackType type;
    public GameObject attackObj;
    private Transform attackPoint;

    public void PerformAttack(){
        if(attackObj != null && attackPoint != null){
            GameObject newAttackObject = Instantiate(attackObj, attackPoint.position, attackPoint.rotation);
            Debug.Log($"Attacked with a {type} attack called {newAttackObject} ");
        } else if (attackObj != null && attackPoint == null){
            attackPoint = transform;
            Vector3 attackOffset = new Vector3(0, 0.5f, 0.5f);
            GameObject newAttackObject = Instantiate(attackObj, attackPoint.position + attackOffset, attackPoint.rotation);
            Debug.Log($"Attacked with a {type} attack called {newAttackObject} ");
        } else return;
    }
    public void AttackSetup(GameObject g, Transform p){
        attackObj = g;
        type = attackObj.GetComponent<BombCopHazrd>().type;
        attackPoint = p;
    }
}

public enum AttackType{
    Melee,
    Bomb,
    Projectile,
}
