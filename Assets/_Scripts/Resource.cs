using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{

    float healthCurrent;
    public float healthMax;
    public bool canHarvest;
    public SliderBar statusBar;

    // Start is called before the first frame update
    void Start()
    {
        StrategyManager.gameAdjudicator.resourceLocations.Add(this);
        healthCurrent = healthMax;
        if(statusBar != null){
            statusBar.SetupBar(healthCurrent, healthMax, this.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        canHarvest = healthCurrent >= 0.1f;
        if(healthCurrent > 0){
            statusBar.UpdateBar(healthCurrent);
        }
    }
    public void Harvest(float harvestAmount){
        if(canHarvest){
            if (healthCurrent > 0)healthCurrent -= harvestAmount; else Destroy(gameObject); StrategyManager.gameAdjudicator.resourceLocations.Remove(this);
        }
    }
}
