using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BomberPlayerController : MonoBehaviour
{
    float speed = 2f;
    float jumpForce = 10f;
    float dashForce = 8f;
    private Vector2 moveInput;
    private Vector3 moveVelocity;
    BombInventoryController inventory;
    public GameObject accessory;
    private CharacterController controller;
    TestControls controls;
    public static event PlayerControllerSpawned onBomberSpawn;
    public delegate void PlayerControllerSpawned (Transform bomber);
    public Transform _groundChecker;
    public LayerMask groundLayer;
    float controllerRotationSmoothing = 1000f;

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
    }
    void OnEnable(){
        controls.Enable();
    }
    void OnDisable(){
        controls.Disable();
    }
    void Update(){
        HandleInput();
    }
    void FixedUpdate(){
        Movement();
    }
    void HandleInput(){
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }
    void Movement()
    {
        moveVelocity = new Vector3 (moveInput.x, 0f, moveInput.y);
        controller.Move(moveVelocity * Time.deltaTime * speed);
        Vector3 playerDirection = Vector3.right*moveInput.x+Vector3.forward*moveInput.y;
        if(playerDirection.sqrMagnitude > 0.0f)
        {
            Quaternion newrotation = Quaternion.LookRotation(playerDirection,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, controllerRotationSmoothing * Time.deltaTime);
        }
        bool isGrounded;
        if (Physics.Raycast(_groundChecker.position, Vector3.down, out RaycastHit hit, 0.55f, groundLayer))
        {
            isGrounded = true;
        }else{
            isGrounded = false;
        }
        if (isGrounded) Debug.DrawRay(_groundChecker.position, Vector3.down, Color.green); else Debug.DrawRay(_groundChecker.position, Vector3.down, Color.red);

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        //moveInput = context.ReadValue<Vector2>();
    }
    
    void Jump()
    {
        // Jump Logic when grounded
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