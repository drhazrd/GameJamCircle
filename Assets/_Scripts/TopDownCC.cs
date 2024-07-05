using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TopDownCC : MonoBehaviour
{
    Rigidbody rigidBody;
    Vector2 movement;
    Vector3 lookPosition;
    Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;
    float forwardAmount;
    float turnAmount;
    public float speed = 4f;
    Animator m_animator;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        SetupAnimator();
    }
    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100)){
            lookPosition = hit.point;
        }
        Vector3 lookDirection = lookPosition - transform.position;
        lookDirection.y = 0;
        transform.LookAt(transform.position + lookDirection, Vector3.up);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(cam != null){
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
            move = vertical * camForward + horizontal * cam.right;
        } else {
            move = vertical * Vector3.forward + horizontal * Vector3.right;
        }
        if(move.magnitude > 1){
            move.Normalize();
        }
        Move(move);

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        rigidBody.AddForce(moveDirection.normalized * speed / Time.deltaTime);


    }
    void Move(Vector3 move){
        if(move.magnitude > 1){
            move.Normalize();
        }
        this.moveInput = move;
        ConvertMoveInput();
        UpdateAnimator();
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
        m_animator = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>()){
            if (childAnimator != m_animator){
                m_animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
            }            
        }
    }
}
