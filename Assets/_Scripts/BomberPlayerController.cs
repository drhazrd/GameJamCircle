using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BomberPlayerController : MonoBehaviour
{
    [Range(0.01f, 1.2f)]
    public float speed = 1.2f;
    
    [Range(1f, 150f)]
    public float jumpForce = 10f;
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
    bool isGrounded, canMove;

    float groundCheckDistance;
    [Range(2f, 5f)]
    public float sprintMultiplier;
    private Vector3 playerDirection;
    private Vector3 moveVelocity;

    public bool isGamePad{get; private set;}

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
        if(accessory != null){
            accessory.SetActive(false);
        }
        animatorFX = GetComponentInChildren<AnimationFX>();
        inventory = GetComponent<BombInventoryController>();
        controller = GetComponent<CharacterController>();
        onBomberSpawn?.Invoke(transform);
        controls.Player.Use.performed += _ => ToggleAccessory();
        controls.Player.Interact.performed += _ => Interact();
        controls.Player.Action.performed += _ => Plant();
        controls.Player.Jump.performed += Jump;
        controls.Player.Sprint.performed += _ => SprintStart();
        controls.Player.Sprint.canceled += _ => SprintStop();
        controls.Player.ItemForward.performed += _ => SwapBomb(1);
        controls.Player.ItemPrevious.performed += _ => SwapBomb(-1);
        controls.Player.ClassUp.performed += _ => SwapDetonator(1);
        controls.Player.ClassDown.performed += _ => SwapDetonator(-1);
        controls.Player.Detonate.performed += _ => Detonate();
    }
    void Update(){
        HandleInput();
        isGrounded = Grounded();
        canMove = BombCopGameManager.Instance.canMove;
        animatorFX.alternate = canMove;
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
        playerDirection = new Vector3 (moveInput.x, 0f, moveInput.y);
        controller.Move(playerDirection * Time.deltaTime * speed);
        if(playerDirection.sqrMagnitude > 0.0f){
            Quaternion newrotation = Quaternion.LookRotation(playerDirection,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }
        moveVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(moveVelocity * Time.deltaTime);
        if(isGrounded && moveVelocity.y < 0){
            moveVelocity.y = 0f;
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
            animatorFX.Interact(); 
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
            inventory.SetBomb();
            animatorFX.Action(); 
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
