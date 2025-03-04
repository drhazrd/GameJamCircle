using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
using UnityEngine.InputSystem;

public class PinballPaddles : MonoBehaviour
{
    public float rotationSpeed = 200.0f; // Speed at which the paddle rotates
    public float maxRotationAngle = 45.0f; // Maximum rotation angle in degrees

    private float currentLeftRotationAngle = 0.0f; // Current rotation angle
    private float currentRightRotationAngle = 0.0f; // Current rotation angle
    public GameObject paddleObjectLeft;
    public GameObject paddleObjectRight;

    private PlayerControls playerInput;
    private InputAction rotateLeftAction;
    private InputAction rotateRightAction;

    void Awake()
    {
        playerInput = new PlayerControls();
        rotateLeftAction = playerInput.Player.RotateLeft;
        rotateRightAction = playerInput.Player.RotateRight;
    }

    void OnEnable()
    {
        rotateLeftAction.Enable();
        rotateRightAction.Enable();
    }

    void OnDisable()
    {
        rotateLeftAction.Disable();
        rotateRightAction.Disable();
    }

    void Update()
    {
        float rotationLeftInput = rotateLeftAction.ReadValue<float>();
        float rotationRightInput = rotateRightAction.ReadValue<float>();
        bool activeLeftPaddle = rotationLeftInput == 1f ? true:false;
        bool activeRightPaddle = rotationRightInput == 1f ? true:false;

        // Calculate rotation angle
        if(activeLeftPaddle){
            float rotationLAmount = rotationLeftInput * rotationSpeed * Time.deltaTime;
            currentLeftRotationAngle = Mathf.Clamp(currentLeftRotationAngle + rotationLAmount, -maxRotationAngle, maxRotationAngle);
        } 
        else{
            currentLeftRotationAngle = 0;
        }
        if(activeRightPaddle){
            float rotationRAmount = rotationRightInput * rotationSpeed * Time.deltaTime;
            currentRightRotationAngle = Mathf.Clamp(currentRightRotationAngle + rotationRAmount, -maxRotationAngle, maxRotationAngle);
        }
        else{
            currentRightRotationAngle = 0;
        }
        // Apply rotation to the paddle
        paddleObjectLeft.transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentLeftRotationAngle);
        paddleObjectRight.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -currentRightRotationAngle);
    }
}
