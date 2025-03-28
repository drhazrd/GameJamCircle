using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RealRacerController : MonoBehaviour
{
    public WheelCollider frontRight, frontLeft, backRight, backLeft;
    public float torqueSpeed = 100f;
    public float maxSteerAngle = 30f;
    bool carReady;
    Vector2 velocityInput;
    TestControls inputs;

    // Start is called before the first frame update
    void Awake()
    {
        inputs = new TestControls();
    }
    void OnEnable()
    {
        inputs.Enable();
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        carReady = CheckCar();
        if (carReady){
            ReadInputs();
        }
    }
    void FixedUpdate()
    {
        if(carReady){
            float motorT = velocityInput.y * torqueSpeed;
            float steerA = velocityInput.x * maxSteerAngle;
            backLeft.motorTorque = motorT;
            backRight.motorTorque = motorT;

            frontLeft.steerAngle = steerA;
            frontRight.steerAngle = steerA;

            if(motorT >= 0.01f) {
                backLeft.brakeTorque = 0;
                backRight.brakeTorque = 0;
            }else{
                backLeft.brakeTorque = 120;
                backRight.brakeTorque = 120;
            }
        }
    }
    private void ReadInputs()
    {
        velocityInput = inputs.Player.Move.ReadValue<Vector2>();
        //velocityInput = Input.GetAxisRaw("Horizontal");
        
    }

    private bool CheckCar()
    {
        bool carCheck = frontLeft != null && frontRight != null && backLeft != null && backRight != null;
        return carCheck;
    }
}
