using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event PlayerDeath onPlayerDeath;
    public delegate void PlayerDeath();
    public float health = 100f;
    public float maxHealth;

    void Awake(){
        health = maxHealth;
    }
    public void ChangeHealth(float change)
    {
        health += change;
         
        if(health <= 0)
        {
            GameManager.Instance.UpdateGameState(GameState.Lose);
            Destroy(gameObject);
            onPlayerDeath.Invoke();
        }
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
