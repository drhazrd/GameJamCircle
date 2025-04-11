using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TopDownCC : MonoBehaviour
{
    CharacterController cc;
    TestControls controls;
    PlayerInput pi;

    Vector2 movement;
    Vector3 lookPosition;
    Camera cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;
    float forwardAmount;
    float turnAmount;
    public float speed = 4f;
    Animator m_animator;
    public GameObject mouseAim;
    public Transform model;
    private bool isGrounded;
    private float velocity;
    private float gravityValue = 6-180;
    private bool isAiming;
    private bool isMoving;
    private float rotationSmoothing = 100f;
    private bool isGamePad;
    private Vector2 aimInput;
    private bool canMove;
    private bool fireGun;

    private void Awake()
    {
        controls = new TestControls();
        pi = GetComponent<PlayerInput>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        model = cc.transform.GetChild(0).transform;
    }
    void Update(){
        HandleInput();
        isGrounded = Grounded();
        BoolChecker();
        if(isAiming) Aim(); else NoAim();

        
    }

    private (bool success, Vector3 position) GetMousePosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, LayerMask.GetMask("Ground"))){
            return (success: true, position: hitInfo.point);
        } else{
            return (success: false, position: Vector3.zero);
        }
    }
    void FixedUpdate()
    {
        Movement();
        Gravity();
    }

    private void Gravity()
    {
        if(isGrounded){
            if(velocity < 0) {
                velocity = -.5f;
            }
        }else {
            velocity += gravityValue * Time.deltaTime;
        }
    }

    void Movement(){
        Vector3 moveVector = new Vector3(moveInput.x * speed, 0, moveInput.y * speed).normalized;
        move.y = velocity;
        move = moveVector * Time.deltaTime;
        cc.Move(move);

    }
    private void HandleInput()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        aimInput = controls.Player.Look.ReadValue<Vector2>();
    }
     private bool Grounded()
    {
        float radius = 0.2f; // Adjust radius to your player's size
        Vector3 groundCheckPosition = transform.position + Vector3.down * (cc.height / 2);
        return Physics.CheckSphere(groundCheckPosition, radius, LayerMask.GetMask("Ground"));
    }
    void ConvertMoveInput(){
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }
    void UpdateAnimator(){
        m_animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        m_animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }
    void SetupAnimator(){
        m_animator = GetComponentInChildren<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>()){
            if (childAnimator != m_animator){
                m_animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
            }            
        }
    }
    void BoolChecker()
    {
        isAiming = aimInput.sqrMagnitude > 0 && canMove;
        isMoving = moveInput.sqrMagnitude > 0.0f && canMove;
    }
    private void NoAim()
    {
        Vector3 inputDirection = new Vector3( moveInput.x, 0, moveInput.y);
        mouseAim.SetActive(false);
        if(moveInput.sqrMagnitude > 0.01f){

            Vector3 lookDirection = inputDirection.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
        }
    }

    private void Aim()
    {
        mouseAim.SetActive(true);
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            model.transform.forward = direction;
            if(mouseAim != null){
                mouseAim.transform.position = position;
            }
        }
    }
    public void OnDeviceChange(PlayerInput pi)
    {
        isGamePad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
        Cursor.visible = !isGamePad;
        if(isGamePad){
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
