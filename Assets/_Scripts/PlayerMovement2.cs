using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement2 : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    float dashForce = 8f;

    private Vector2 m_input;
    private Vector3 move;
    BombInventoryController inventory;
    private CharacterController controller;
    TestControls controls;
    AnimationFX animatorJuice;

    public float gravityValue = -9.81f;

    private float controllerRotationSmoothing = 1000f;
    private bool groundedPlayer, canMove;

    float groundCheckDistance;
    [Range(2f, 5f)]
    public float sprintMultiplier;
    private Vector3 playerVelocity;
    
    bool isGamepad;

    public static event Action onDetonateAllBombs;

    public bool isMoving{get; private set;}
    public bool isSprinting{get; private set;}

    void Awake(){
        controls = new TestControls();
    }

    void OnEnable(){
        controls.Enable();
    }

    void OnDisable(){
        controls.Disable();
    }

    void Start(){
        controller = GetComponent<CharacterController>();
        controls.Player.Jump.performed += OnJump; // Subscribe to the Jump action event
        animatorJuice = GetComponentInChildren<AnimationFX>();
        inventory = GetComponent<BombInventoryController>();
        controls.Player.Interact.performed += _ => Interact();
        controls.Player.Action.performed += _ => DoAction();
        controls.Player.Sprint.performed += _ => SprintStart();
        controls.Player.Sprint.canceled += _ => SprintStop();
    }

    void Update(){
        groundedPlayer = Grounded();
        canMove = GameManager.Instance.canMove;
        if(canMove){
            HandleInput();
            isMoving = m_input != Vector2.zero;
        }
    }


    void FixedUpdate(){
        if(canMove){
            Mover();
        }
    }
    private void DoAction()
    {
        //Action
    }

    private void HandleInput()
    {
        m_input = controls.Player.Move.ReadValue<Vector2>();
    }

    private void Mover(){
        move = new Vector3(m_input.x, 0f, m_input.y);
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (move != Vector3.zero){
            gameObject.transform.forward = move;
        }
        if(move.sqrMagnitude > 0.0f){
            Quaternion newrotation = Quaternion.LookRotation(move,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        if (groundedPlayer && playerVelocity.y < 0){
            playerVelocity.y = 0f;
        }
    }

    bool Grounded(){
        float bufferCheckDistance = 0.1f;
        groundCheckDistance = (controller.height / 2) + bufferCheckDistance;
        RaycastHit hit;
        if(Physics.Raycast (transform.position, - transform.up, out hit, groundCheckDistance)){
            return true;
        } else 
        return false;
    }

    void OnJump(InputAction.CallbackContext context){
        if (groundedPlayer && canMove)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    void Dash(){
        if(canMove){
        }
    }

    void SprintStart(){
        isSprinting = true;
        playerSpeed *= sprintMultiplier;      
    }

    void SprintStop(){
        isSprinting = false;
        playerSpeed /= sprintMultiplier;
    }

    void Interact(){}

}