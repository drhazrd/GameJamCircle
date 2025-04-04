using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TestPlayer : MonoBehaviour
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
    private float gravityValue = -180f;
    public float gravityMultiplier = 1f;
    AnimationFX animatorFX;


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
        animatorFX = GetComponentInChildren<AnimationFX>();
    }

    private void Jump()
    {
        if(!isGrounded) return; else velocity += jumpPower;
        if(animatorFX != null) animatorFX.Jump();

    }

    void Update(){
        HandleInput();
        isGrounded = Grounded();
        if(animatorFX != null) animatorFX.isGrounded = isGrounded;
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
            Animation();
        } 
    }

    private void Animation()
    {
        bool isMoving = moveInput != Vector2.zero;
        if(animatorFX != null){
            animatorFX.isMoving = isMoving;
        }
    }

    private void Gravity()
    {
        if(isGrounded){
            if(velocity < 0) 
                velocity = -.5f;
        }else {
            velocity += gravityValue * Time.deltaTime;
        }
        playerDirection.y = velocity;
    }

    private void Movement()
    {
        playerDirection = new Vector3 (moveInput.x * speed, velocity, moveInput.y * speed);
        if(playerDirection.sqrMagnitude > 0.01f){
            Quaternion newrotation = Quaternion.LookRotation(new Vector3(playerDirection.x, 0f, playerDirection.z),Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }
        controller.Move(playerDirection * Time.deltaTime);
    }
}
