using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CatGameManager : MonoBehaviour
{
    int catCoins;
    int days;
    float time;
    public List<CatBehaviour> currentCats;
    public bool paused;

    public TextMeshProUGUI gameData;
    
    void OnEnable(){
        CatBehaviour.onCatSpawn += CatRegister;
    }

    void OnDisable(){
        CatBehaviour.onCatSpawn -= CatRegister;
    }


    void Awake(){
        days = 0;
        time = 0;
        currentCats = new List<CatBehaviour>();
    }

    void Update(){
        if(!paused) time += Time.deltaTime; else return;
        gameData.text = time.ToString("00");
    }
    void CatRegister(CatBehaviour cat){
        currentCats.Add(cat);
    }
}
