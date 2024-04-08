using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    Image meterImage;
    float maxMeter;
    float currentMeter;
    bool isFilled;

    void Awake(){
        meterImage = GetComponent<Image>();
    }

    public void SetMeter(float max, float current){
        currentMeter = current;
        maxMeter = max;
        meterImage.fillAmount = max / current;
    }
    
    public void UpdateMeter(float max, float current){
        currentMeter = current;
        meterImage.fillAmount = max / current;
    }
}
