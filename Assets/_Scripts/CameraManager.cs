using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera virtualCam;

    void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void UpdateCamTarget(Transform t)
    {
        virtualCam.Follow = t;
    }
}
