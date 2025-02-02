using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    private float sprintSpeed;
    public int maxJumps = 2;  // Maximum number of jumps allowed
    int jumpCount = 0;  // Current number of jumps
    private float verticalRotation = 90f;
    private CharacterController characterController;
    private Camera cam;
    private Vector3 velocity;
    private Vector2 look;
    private bool Sprint;
    private bool Jumping;
    private bool _isGrounded;
    InputManager playerInput;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        verticalRotation = 0;
        playerInput = GetComponent<InputManager>();
        playerInput.controls.Movement.Jump.started += _ => Jump();
        Cursor.lockState = CursorLockMode.Locked;
        sprintSpeed = movementSpeed * 2;
    }

    void Update()
    {
        _isGrounded = IsGrounded();

        // Reset jump count if grounded
        if (_isGrounded)
        {
            jumpCount = 0;
        }

        if(Input.GetButtonDown("Sprint")){
            Sprint = true;
            Debug.Log("Sprinting True");
        } else if(Input.GetButtonUp("Sprint")){
            Sprint = false;
            Debug.Log("Sprinting False");
        }

        CameraControl();
        PlayerMovement();
        GravityApplied();
        characterController.Move(velocity * Time.deltaTime);
    }

    void CameraControl()
    {
        float mouseX;
        float mouseY;
        //look = playerInput.controls.CameraLook.PadLook.ReadValue<Vector2>();
        bool isGamepad = false;

        if(playerInput != null){
            
            if(!isGamepad){
                mouseX = playerInput.controls.CameraLook.MouseX.ReadValue <float>() * mouseSensitivity * Time.deltaTime;
                mouseY = playerInput.controls.CameraLook.MouseY.ReadValue <float>() * mouseSensitivity * Time.deltaTime;
            } else {
                mouseX = look.x * Time.deltaTime;
                mouseY = look.y * Time.deltaTime;
            }
        } else {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        }

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void GravityApplied()
    {
        if (!Jumping)
        {
            velocity.y = -0.5f; // Reset vertical velocity when grounded
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    void PlayerMovement()
    {
        float moveX;
        float moveZ;
        float currentSpeed = Sprint ? sprintSpeed : movementSpeed;
        if (playerInput != null)
        {
            moveX = playerInput.controls.Movement.Right.ReadValue<float>() * currentSpeed * Time.deltaTime;
            moveZ = playerInput.controls.Movement.Forward.ReadValue<float>() * currentSpeed * Time.deltaTime;
        }
        else
        {
            moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        }

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(movement);
    }

    void Jump(){
        if(maxJumps > jumpCount){
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            Jumping = true;
            jumpCount ++;
            Debug.Log("Jump");}
    }

    private bool IsGrounded()
    {
        float raycastDistance = characterController.skinWidth + 1.1f;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(raycastOrigin, Vector3.down, raycastDistance);
    }
}
