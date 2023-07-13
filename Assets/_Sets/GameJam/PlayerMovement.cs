using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator _anim;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isMoving;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            playerVelocity.y = 0f; // Reset the vertical velocity when grounded
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            transform.forward = move;
            _anim.SetBool("isRunning", true);
            Debug.Log("Move");
        }
        else{
            _anim.SetBool("isRunning", false);
            Debug.Log("No move");
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}