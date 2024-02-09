using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera virtualCam;
    
    void OnEnable(){
        PlayMove2DLicht.onPlayerSpawn += UpdateCamTarget;
    }
    
    void OnDisable(){
        PlayMove2DLicht.onPlayerSpawn -= UpdateCamTarget;
    }

    void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update(){

    }

    void UpdateCamTarget(Transform t)
    {
        virtualCam.Follow = t;
    }
}
