using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JourneyMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Transform cam;
    private Vector2 moveInput;
    private float turnSmoothVelocity;
    private float verticalVelocity;
    private Animator _anim;
    public bool isMoving {get; private set;}
    private bool isJumping;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        AnimateCharacter();
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        isMoving = direction == new Vector3(0,0) ? false:true;
        if (controller.isGrounded)
        {
            verticalVelocity = 0; // Reset vertical velocity when grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity to vertical velocity
        }
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        Vector3 gravityMovement = new Vector3(0, verticalVelocity, 0);
        controller.Move(gravityMovement * Time.deltaTime);
    }

    void AnimateCharacter(){
        if(_anim){
            _anim.SetBool("Walking", isMoving);
        }
    }
}
