using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int cost;
    public int value;
    public float timeToBuild;
    public BuildingType type;
}
public enum BuildingType{
    Resource,
    Harvest,
    Combat, 
    Defense
}