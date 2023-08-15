using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    private float verticalRotation = 0f;
    private CharacterController characterController;
    private Camera cam;
    private Vector3 velocity;
    private bool isJumping;
    private bool isGrounded;
    private bool _isGrounded;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        verticalRotation = 0;
    }

    void Update()
    {
        _isGrounded = IsGrounded();
        isGrounded = characterController.isGrounded;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Player movement
        float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(movement);

         // Jumping
        if (characterController.isGrounded)
        {
            isJumping = false;
            velocity.y = -0.5f; // Reset vertical velocity when grounded
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
                isJumping = true;
            }
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private bool IsGrounded()
    {
        float raycastDistance = characterController.skinWidth + 1.5f;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(raycastOrigin, Vector3.down, raycastDistance);
    }
}
