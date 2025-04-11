using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentTank : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    CharacterController cc;
    private Camera mainCamera;
    private Transform _model;
    TestControls controls;
    Vector2 moveInput;
    float speed = 3f;
    public bool isAiming;
    private bool isGrounded;
    private float velocity;
    private float gravityValue;
    Vector3 moveVelocity;
    private float rotationSmoothing = 100f;

    private void Awake()
    {
        controls = new TestControls();
    }
    private void Start()
    {
        // Cache the camera, Camera.main is an expensive operation.
        mainCamera = Camera.main;
        _model = transform.GetChild(0).transform;
        cc = GetComponent<CharacterController>();
    }

    private void Update(){
        HandleInput();
    }

    private void HandleInput()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if(isAiming) Aim(); else NoAim();
        
        Gravity();
        Movement();
    }


    private void Gravity()
    {
        if(isGrounded){
            if(velocity < 0) 
                velocity = -.5f;
        }else {
            velocity += gravityValue * Time.deltaTime;
        }
        moveVelocity.y = velocity;
    }
    private void Movement()
    {
        Vector3 movementVelocity = new Vector3(moveInput.x * speed * Time.deltaTime, 0, moveInput.y * speed * Time.deltaTime).normalized;
        
        cc.Move(movementVelocity);
    }
    private void NoAim()
    {
        Vector3 inputDirection = new Vector3( moveInput.x, 0, moveInput.y);
        if(moveInput.sqrMagnitude > 0.01f){

            Vector3 lookDirection = inputDirection.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
        }
    }
    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            _model.transform.forward = direction;
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

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
