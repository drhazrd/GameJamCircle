using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BomberPlayerController : MonoBehaviour
{
    float speed = 2f;
    float jumpForce = 1f;
    float dashForce = 8f;
    private Vector2 moveInput;
    private Vector3 moveVelocity;
    BombInventoryController inventory;
    public GameObject accessory;
    private CharacterController controller;
    TestControls controls;

    public static event PlayerControllerSpawned onBomberSpawn;
    public delegate void PlayerControllerSpawned (Transform bomber);
    private float gravityValue = -9.81f;


    float controllerRotationSmoothing = 1000f;
    bool isGrounded;

    float groundedGravity = -0.1f;
    float groundCheckDistance;
    
    void Awake(){
        controls = new TestControls();
    }
    void Start()
    {
        if(accessory != null){
            accessory.SetActive(false);
        }
        inventory = GetComponent<BombInventoryController>();
        controller = GetComponent<CharacterController>();
        onBomberSpawn?.Invoke(transform);
        controls.Player.Use.performed += _ => ToggleAccessory();
        controls.Player.Interact.performed += _ => Interact();
        controls.Player.Action.performed += _ => Plant();
        controls.Player.Jump.performed += _ => Jump();
    }
    void OnEnable(){
        controls.Enable();
    }
    void OnDisable(){
        controls.Disable();
    }
    void Update(){
        HandleInput();
        isGrounded = Grounded();
        
        if (isGrounded)
        {
            moveVelocity.y = groundedGravity; // Apply small downward force when grounded
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red); 
        }
        else
        {
            moveVelocity.y += gravityValue * Time.deltaTime; // Apply gravity when not grounded
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green); 
        }
    }
    void FixedUpdate(){
        Movement();
    }
    void HandleInput(){
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }
    void Movement()
    {
        Vector3 playerDirection = new Vector3 (moveInput.x, 0f, moveInput.y);      
        controller.Move(playerDirection * Time.deltaTime * speed);
        if(playerDirection.sqrMagnitude > 0.0f)
        {
            Quaternion newrotation = Quaternion.LookRotation(playerDirection,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }

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
    public void OnMove(InputAction.CallbackContext context)
    {
        //moveInput = context.ReadValue<Vector2>();
    }
    
    void Jump()
    {
        if(isGrounded){
            Debug.Log(jumpForce * -3.0f * gravityValue);
            moveVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
        }
    }
    
    void Dash()
    {
        //Dash forward only on input
    }
    
    void Interact()
    {
        //On interact button press activate the interact button 
    }

    void ToggleAccessory()
    {
        if(accessory != null){
            bool newState = accessory.activeInHierarchy;
            accessory.SetActive(!newState); 
        }
    }

    void Plant()
    {
        if(inventory != null)inventory.SetBomb();
    }
    
    void Swap(int id)
    {
        if(inventory != null) inventory.SetBombID(id);
    }
}
