using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement2 : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    private Vector3 move;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        move = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
    }

    public void Jump(InputAction.CallbackContext ctx){
        if(ctx.performed && groundedPlayer)
        {
            Debug.Log("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    void Update()
    {

        
        groundedPlayer = controller.isGrounded;
        
        //if (groundedPlayer && playerVelocity.y < 0)
        {
            //playerVelocity.y = 0f;  
        }
        

        
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        
        
        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;  
        }
    }
}
