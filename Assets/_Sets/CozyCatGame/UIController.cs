using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController ui;
    public Meter[] statMeters;
    void Awake()
    {
        if(UIController.ui != this && UIController.ui==null){
            ui = this;
        } else{
            Destroy(this);
        }
    }

    void Start()
    {
        statMeters = GetComponentsInChildren<Meter>();
    }

    public void UpdateStats(){
        for(int i = 0; i > statMeters.Length; i++){
            statMeters[i].UpdateMeter(0f, 0f);
        }
    }
}
