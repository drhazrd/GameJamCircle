using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class  RoomExit : MonoBehaviour
{
    public int levelIntToLoad;
    public string levelStringToLoad;
    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player"){
            if(levelIntToLoad > 0){
                SceneManager.LoadScene(levelIntToLoad);
            }else if(levelStringToLoad != null && levelStringToLoad != ""){
                SceneManager.LoadScene(levelStringToLoad);
            }
            else return;
        }
    }
}