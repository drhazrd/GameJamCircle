using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator _anim;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isMoving;
    private bool isSprinting;
    private float playerSpeed = 2.0f;
    private float walkSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private float groundedGravity = -0.1f;
    Vector2 moveInput;
    PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        playerControls.Player.Jump.performed += ctx => Jump();
        playerControls.Player.Jump.canceled += ctx => Jump();
        playerControls.Player.Action.performed += ctx => Sprint();
    }
    void OnEnable(){
        playerControls.Enable();
    }
    void OnDisable(){
        playerControls.Disable();
    }


    void FixedUpdate(){
        Movement();
    }

    void Update()
    {
        isGrounded = Grounded();
        isMoving = moveInput != Vector2.zero;
        if (isGrounded)
        {
            playerVelocity.y = groundedGravity; // Apply small downward force when grounded
        }
        else
        {
            playerVelocity.y += gravityValue * Time.deltaTime; // Apply gravity when not grounded
        }
        HandleInput();
        AnimationHandling();
    }
    void Movement(){
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        if(move != Vector3.zero){
            transform.forward = move;
        }
        controller.Move(move * Time.deltaTime * playerSpeed);
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void Jump(){
        Debug.Log("jump");
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }
    void AnimationHandling(){
        _anim.SetBool("Sprint", isSprinting);
        _anim.SetBool("isRunning", isMoving);
    }
    void Sprint(){
        Debug.Log("sprint");
        isSprinting = !isSprinting;
        if (isGrounded && isSprinting)
        {
            playerSpeed = walkSpeed * 2f;

        }else if (isGrounded){
            playerSpeed = walkSpeed;
        } 
    }
    bool Grounded(){
        float groundCheckDistance;
        float bufferCheckDistance = 0.1f;

        groundCheckDistance = (controller.height / 2) + bufferCheckDistance;

        RaycastHit hit;
        if(Physics.Raycast (transform.position, - transform.up, out hit, groundCheckDistance)){
            return true;
        } else 
        return false;
    }
    void HandleInput(){
        moveInput = playerControls.Player.Move.ReadValue<Vector2>();
    }
}