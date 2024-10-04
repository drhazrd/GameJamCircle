using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderBar : MonoBehaviour
{
    public float currentValue;
    public float maxValue = 1f;
    public GameObject barCanvasObject;
    Image barImage;
    TextMeshProUGUI barImageText;

    void Awake(){
        if(barCanvasObject != null){
            barImage = GetComponentInChildren<Image>();
            barImageText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void SetupBar(float c, float m, string t){
        maxValue = m;
        currentValue = maxValue;
        float value = c/m;
        bool showBar = currentValue > 0 ? true:false;
        barCanvasObject.SetActive(showBar);
        barImage.fillAmount = value;
        barImageText.text = $"{t}";
    }
    public void UpdateBar(float newValue){
        currentValue = newValue;
        float value = currentValue/maxValue;
        bool showBar = currentValue > 0 ? true:false;
        barCanvasObject.SetActive(showBar);
        barImage.fillAmount = value;
    }
}