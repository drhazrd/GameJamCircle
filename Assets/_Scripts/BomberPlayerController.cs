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
    private CharacterController controller;

    void Start()
    {
        inventory = GetComponent<BombInventoryController>();
        controller = GetComponent<CharacterController>();
    }

    void Update(){
        HandleInput();
    }
    void HandleInput(){
        Movement();
    }
    void Movement()
    {
        moveVelocity = new Vector3 (moveInput.x, 0f, moveInput.y);
        controller.Move(moveVelocity);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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

    void Plant()
    {
        if(inventory != null)inventory.SetBomb();
    }
    
    void Swap(int id)
    {
        if(inventory != null) inventory.SetBombID(id);
    }
}
