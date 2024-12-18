using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyrCtrl : MonoBehaviour {

    CharacterController controller;
    Animator _anim;
    private Vector3 moveDirection;
    public float moveSpeed;
    float distFromGround;
    public float jumpForce;
    public float gravityScale;

    public Transform pivey;
    public float rotateSpeed;
    public GameObject model;
    public bool canMove;
    public bool m_grounded;
   
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;



    void Start () {
        controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();

	}

	void Update () {
        m_grounded = controller.isGrounded;
        if (knockBackCounter <= 0)
        {
            float yStore = moveDirection.y;
            if (canMove == true)
            {
                moveDirection = (transform.forward) + (transform.right * Input.GetAxis("Horizontal"));
                moveDirection = moveDirection.normalized * moveSpeed;
            }
            else { return; }
            moveDirection.y = yStore;
            if (controller.isGrounded)
            {
                moveDirection.y = 0f;
                //Improved jump script.
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }

            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }

		//Apply gravity.
		moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

		//Apply vector.
		controller.Move(moveDirection * Time.deltaTime);

        distFromGround = moveDirection.y;
        _anim.SetFloat("distanceFromGround", distFromGround);


        //move the player in different directions based on the camera

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivey.rotation.eulerAngles.y, 0);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            _anim.SetBool("isRunning", true);
        }
        else if (Input.GetAxis("Horizontal") == 0f || Input.GetAxis("Vertical") == 0f)
        {
            _anim.SetBool("isRunning", false);
        }

		_anim.SetBool ("isGrounded", controller.isGrounded);
		_anim.SetFloat ("runSpeed", (Mathf.Abs (Input.GetAxis ("Vertical"))+ Mathf.Abs(Input.GetAxis("Horizontal"))));

	}
   
    public void Jump()
    {
        moveDirection.y = jumpForce;
        _anim.SetTrigger("Jump");
    }
    public void Knockback(Vector3 direction)
    {
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }

}﻿



