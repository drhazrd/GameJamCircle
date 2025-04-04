using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement2 : MonoBehaviour
{
    [Range(0.01f, 12f)]
    public float speed = 6f;
    public float jumpPower = 6f;
    TestControls controls;
    private Vector2 moveInput;
    private CharacterController controller;
    public bool isGrounded{get; private set;}
    private Vector3 playerDirection;
    float velocity;
    private bool canMove = true;
    private float controllerRotationSmoothing = 1000f;
    private float gravityValue = -98.1f;
    public float gravityMultiplier = 10f;


    void Awake(){
        controls = new TestControls();
    }

    void OnEnable(){
        controls.Enable();
    }

    void OnDisable(){
        controls.Disable();
    }
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controls.Player.Jump.performed += _ => Jump();

    }

    private void Jump()
    {
        if(!isGrounded) return;
        velocity += jumpPower;
    }

    void Update(){
        HandleInput();
        isGrounded = Grounded();
    }

    private bool Grounded()
    {
        float radius = 0.2f; // Adjust radius to your player's size
        Vector3 groundCheckPosition = transform.position + Vector3.down * (controller.height / 2);
        return Physics.CheckSphere(groundCheckPosition, radius, LayerMask.GetMask("Ground"));
    }

    private void HandleInput()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if(canMove){
            Movement();
            Gravity();
        } 
    }

    private void Gravity()
    {
        if(isGrounded){
            if(velocity < 0) 
                velocity = -.5f;
        }else {
            velocity += gravityValue * gravityMultiplier * Time.deltaTime;
        }
        playerDirection.y = velocity;
    }

    private void Movement()
    {

        playerDirection = new Vector3 (moveInput.x * speed, 0f, moveInput.y * speed);
        if(playerDirection.sqrMagnitude > 0.01f){
            Quaternion newrotation = Quaternion.LookRotation(new Vector3(playerDirection.x, 0f, playerDirection.z),Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }
        controller.Move(playerDirection * Time.deltaTime);
    }
}
