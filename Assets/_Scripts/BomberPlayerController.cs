using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class BomberPlayerController : MonoBehaviour
{
    [Range(0.01f, 1.2f)]
    public float speed = 1.2f;
    
    [Range(.1f, .9f)]
    public float jumpForce = .1f;
    [Range(2f, 50f)]
    public float kickForce = 10f;
    float dashForce = 8f;

    private Vector2 moveInput;
    BombInventoryController inventory;
    public GameObject accessory;
    private CharacterController controller;
    TestControls controls;
    AnimationFX animatorFX;

    public static event PlayerControllerSpawned onBomberSpawn;
    public delegate void PlayerControllerSpawned (Transform bomber);
    public float gravityValue = -9.81f;


    float controllerRotationSmoothing = 1000f;
    public bool isGrounded{get; private set;}
    public bool canMove{get; private set;}

    float groundCheckDistance;
    [Range(2f, 5f)]
    public float sprintMultiplier;
    private Vector3 playerDirection;
    private Vector3 moveVelocity;

    public bool isGamePad{get; private set;}

    public static event DetonateAllBombs onDetonateAllBombs;
    public delegate void DetonateAllBombs();

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
        if(accessory != null){
            accessory.SetActive(false);
        }
        animatorFX = GetComponentInChildren<AnimationFX>();
        inventory = GetComponent<BombInventoryController>();
        controller = GetComponent<CharacterController>();
        onBomberSpawn?.Invoke(transform);
        controls.Player.Use.performed += _ => Hold();
        controls.Player.Use.canceled += _ => Throw();
        controls.Player.Interact.performed += _ => Interact();
        controls.Player.Action.performed += _ => Plant();
        controls.Player.Jump.performed += _ => Plant();
        controls.Player.Sprint.performed += _ => SprintStart();
        controls.Player.Sprint.canceled += _ => SprintStop();
        controls.Player.ItemForward.performed += _ => SwapBomb(1);
        controls.Player.ItemPrevious.performed += _ => SwapBomb(-1);
        controls.Player.ClassUp.performed += _ => SwapDetonator(1);
        controls.Player.ClassDown.performed += _ => SwapDetonator(-1);
        controls.Player.Detonate.performed += _ => Detonate();
    }

    private void Hold(){
        if (inventory != null) inventory.HoldBomb();
    }
    private void Throw()
    {
        if (inventory != null) inventory.PushBomb(transform, kickForce);
    }

    void Update(){
        HandleInput();
        isGrounded = Grounded();
        canMove = BombCopGameManager.Instance.canMove;
        if(animatorFX != null) animatorFX.alternate = canMove;
        if(canMove){
            isMoving = moveInput != Vector2.zero;
        }
    }
    void FixedUpdate(){
        if(canMove){
            Movement();
        } 
    }
    void HandleInput(){
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }
    void Movement(){
        if(isGrounded && moveVelocity.y < 0){
            moveVelocity.y = 0f;
        }else moveVelocity.y += gravityValue * Time.deltaTime;
        playerDirection = new Vector3 (moveInput.x, 0, moveInput.y);
        if(playerDirection.sqrMagnitude > 0.0f){
            Quaternion newrotation = Quaternion.LookRotation(playerDirection,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }
        
        controller.Move(playerDirection * Time.deltaTime * speed);
        controller.Move(moveVelocity * Time.deltaTime);
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
    
    void Jump(InputAction.CallbackContext context){
        if(isGrounded && canMove){
            moveVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
        }
    }
    
    public void BombJump(int power){
        if (isGrounded && canMove)
        {
            moveVelocity.y += Mathf.Sqrt(jumpForce* -3.0f * gravityValue);
        }
        if(animatorFX != null) animatorFX.Jump();
    }

    void Dash(){
        if(canMove){
        }
    }
    void SprintStart(){
        isSprinting = true;
        speed *= sprintMultiplier;      
    }
    void SprintStop(){
        isSprinting = false;
        speed /= sprintMultiplier;
    }
    
    void Interact(){
        if(canMove){
            if(animatorFX != null) animatorFX.Interact(); 
        }
    }

    void ToggleAccessory(){
        if(accessory != null && canMove){
            bool newState = accessory.activeInHierarchy;
            accessory.SetActive(!newState); 
        }
    }

    void Plant(){
        if(inventory != null && canMove){
            if(inventory != null) inventory.SetBomb();
            if(animatorFX != null) animatorFX.Action(); 
        }
    }
    
    void SwapBomb(int id){
        if(inventory != null && canMove) inventory.SetBombID(inventory.bombTypeID + id);
    }

    void SwapDetonator(int id){
        if(inventory != null && canMove) inventory.SetDetonatorTypeID(inventory.bombDetonatorID + id);
    }

    void Detonate(){
        if(canMove){
            onDetonateAllBombs?.Invoke();
        }
    }

    public void FaceDirection(Vector3 position){
        //throw new NotImplementedException();
    }
}
public enum ControlButton{
    Interact,
    Action,
    Use,
    Jump,
    Detonate,
    CycleLR,
    CycleUD,
    WhipTrigger,
    Move
}
