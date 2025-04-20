using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager iManager;
    public GameObject pauseMenuObject, interactObject, hudObject, timerObject, eventObject, statsObject;

    void Awake(){
        if(iManager != null && UIManager.iManager != this){
            Destroy(this);
        }
        else iManager = this;
    }

    public void InteractButtonDisplay(string interact)
    {
        if(interactObject != null) {
            interactObject.SetActive(true);
            interactObject.GetComponentInChildren<TextMeshProUGUI>().text = interact;
        }
    }
    public void InteractButtonDisplay()
    {
        if(interactObject != null) interactObject.SetActive(true);
    }

    public void InteractButtonHide()
    {
        if(interactObject != null) interactObject.SetActive(false);
    }
    public void HUDDisplay()
    {
        if(hudObject != null) hudObject.SetActive(true);
    }

    public void HUDHide()
    {
        if(hudObject != null) hudObject.SetActive(false);
    }
}
