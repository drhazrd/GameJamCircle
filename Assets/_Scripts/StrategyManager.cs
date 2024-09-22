using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StrategyManager : MonoBehaviour
{
    float gameTime;
    public TextMeshProUGUI gameTimeText;
    void Start()
    {
        gameTime = 0;
    }

    
    void Update(){
        gameTime += Time.deltaTime;
        GameTime(gameTime);
    }
    void GameTime(float info)
    {
        if(info >= 0){

            int minutes = Mathf.FloorToInt(info / 60);
            int seconds = Mathf.FloorToInt(info % 60);
            gameTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
