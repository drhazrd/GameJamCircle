using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Mission Objective", menuName="Object Data/Mission")]

public class MissionObjectives : ScriptableObject
{
    public int levelRequired;
    public string missionName, missionDescription;
    public ItemType collectableNeeded;
    public int requiredCount = 10;
    public int missionReward, missionExp;
}
