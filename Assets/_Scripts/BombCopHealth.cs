using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCopHealth : Damageable
{
    public override void Die(){
        GameObject model = transform.GetChild(0).gameObject;
        model.SetActive(false);
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        BombCopUIManager.ui.UpdatePlayerStats(health, maxHealth);
    }
}
