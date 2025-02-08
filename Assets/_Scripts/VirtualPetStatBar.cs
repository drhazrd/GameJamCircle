using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPetStatBar : MonoBehaviour
{
    
    public TextMeshProUGUI statName;
    public Image statBarImage;
    void Start()
    {
        statName = GetComponentInChildren<TextMeshProUGUI>();
        statBarImage = GetComponentInChildren<Image>();
    }

    public void UpdateBar(float c, float m)
    {
        statBarImage.fillAmount = c / m;
    }
}
