using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TopDownCC : MonoBehaviour
{
    public event Action onActionPressed;
    public event Action onInteractPressed;
    public event Action onUsePressed;    

    [Range(0.01f, 12f)]
    public float speed = 6f;
    public float jumpPower = 6f;
    TestControls controls;
    PlayerInput pi;
    private Vector2 moveInput;
    private Vector2 aimInput;
    private CharacterController controller;
    public bool isGrounded{get; private set;}
    public bool isAiming{get; private set;}
    public bool isMoving{get; private set;}
    public bool isGamePad{get; private set;}
    private Vector3 playerDirection;
    float velocity;
    public bool canMove {get; private set;}
    private float controllerRotationSmoothing = 1000f;
    private float gravityValue = -180f;
    public float gravityMultiplier = 1f;
    AnimationFX animatorFX;
    Transform _model;
    private GameObject mouseAim;
    private float rotationSmoothing = 1000f;

    void Awake(){
        controls = new TestControls();
        pi = GetComponent<PlayerInput>();
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
        controls.Player.Action.performed += _ => Action();
        controls.Player.Interact.performed += _ => Interact();
        controls.Player.Use.performed += _ => Use();
        animatorFX = GetComponentInChildren<AnimationFX>();
        _model = animatorFX.transform;
    }

    private void Jump()
    {
        if(!isGrounded) return; else velocity += jumpPower;
        if(animatorFX != null) animatorFX.Jump();

    }

    void Update(){
        HandleInput();
        BoolChecker();
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
        aimInput = controls.Player.Look.ReadValue<Vector2>();

    }

    void FixedUpdate()
    {
        if(canMove){
            Movement();
            Gravity();
            Animation();
            if(isAiming) Aim(); else NoAim();

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
    }

    private void Movement()
    {
        playerDirection = new Vector3 (moveInput.x * speed, 0, moveInput.y * speed);
        playerDirection.y = velocity;
        controller.Move(playerDirection * Time.deltaTime);
    }
    void Interact(){
        if(animatorFX != null) animatorFX.Interact();
        onInteractPressed?.Invoke();
    }
    void Action(){
        if(animatorFX != null) animatorFX.Action(); 
        onActionPressed?.Invoke();
    }
    void Use(){
        if(animatorFX != null) animatorFX.Use(); 
        onUsePressed?.Invoke();
    }
    public void OnDeviceChange(PlayerInput pi)
    {
        isGamePad = pi.currentControlScheme.Equals("GamePad") ? true : false;
        Cursor.visible = !isGamePad;
        if(isGamePad){
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    private void NoAim()
    {
        Vector3 inputDirection = new Vector3( moveInput.x, 0, moveInput.y);
        if(mouseAim != null)mouseAim.SetActive(false);
        if(moveInput.sqrMagnitude > 0.01f){

            Vector3 lookDirection = inputDirection.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
        }
    }

    private void Aim()
    {
        if(!isGamePad){
            if(mouseAim != null) mouseAim.SetActive(true);
            var (success, position) = GetMousePosition();
            if (success)
            {
                var direction = position - transform.position;
                direction.y = 0;
                _model.transform.forward = direction;
                if(mouseAim != null){
                    mouseAim.transform.position = position;
                }
            }
        } else {
            Vector3 inputDirection = new Vector3( aimInput.x, 0, aimInput.y);
            if(mouseAim != null)mouseAim.SetActive(false);
            if(aimInput.sqrMagnitude > 0.01f){

                Vector3 lookDirection = inputDirection.normalized;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
            }
        }
    }
    private (bool success, Vector3 position) GetMousePosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground"))){
            return (success: true, position: hitInfo.point);
        } else{
            return (success: false, position: Vector3.zero);
        }
    }
        void BoolChecker()
    {
        isGrounded = Grounded();
        if(animatorFX != null) animatorFX.isGrounded = isGrounded;
        canMove = GameManager.gameManager.canMove;
        isAiming = aimInput.sqrMagnitude > 0 && canMove;
        isMoving = moveInput.sqrMagnitude > 0.0f && canMove;
    }
}
