using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls controls;
    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable(){
        controls.Enable();
    }
    void OnDisable(){
        controls.Disable();
    }
   
}
