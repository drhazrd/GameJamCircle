using System.Collections;
using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
        
    public static PlayerStats stats;
    public static event Action onLevelUp;
    
    public int currentLevel = 1;
    public int currentExp = 0;
    public int nextLevelExp;
    public int previousExp;
    public int totalKills;

     void Awake(){
        if (stats != this && PlayerStats.stats != null){Destroy(this);}else{ stats = this;}
    }
    void Start(){
        //LoadStats();
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetNeedExp(int currentLvl)
    {
        if (currentLvl == 0)
            return 0;
        return (currentLvl * currentLvl + currentLvl) * 5;
    }
    public void SetExperience(int exp){
        currentExp += exp;
        nextLevelExp = GetNeedExp(currentLevel);
        previousExp = GetNeedExp(currentLevel - 1);

        if(currentExp >= nextLevelExp){
            LevelUp();
            onLevelUp?.Invoke();
            nextLevelExp = GetNeedExp(currentLevel);
            previousExp = GetNeedExp(currentLevel - 1);
        }
    }


    public void LevelUp()
    {
        if (currentLevel >= 60) currentLevel = 60; else currentLevel++;
        Debug.Log($"Player Level {currentLevel}");
    }
}
