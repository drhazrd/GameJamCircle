using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StrategyManager : MonoBehaviour
{
    public static StrategyManager gameAdjudicator { get; private set; }

    float gameTime;
    public TextMeshProUGUI gameTimeText;
    public Transform mainBaseLocation;
    public Resource[] resourceLocations;

    private void Awake()
    {
        if (gameAdjudicator == null)
        {
            gameAdjudicator = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
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
