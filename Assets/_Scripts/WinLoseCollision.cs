using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseCollision : MonoBehaviour
{
    public string nextLevel = "Game";    
    public AudioClip soundEffect;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {   
            Debug.Log("Complete");  
            AudioManager.instance.PlayClip(soundEffect);
            GameManager.Instance.UpdateGameState(GameState.Win);
        }        
    }
    
}
