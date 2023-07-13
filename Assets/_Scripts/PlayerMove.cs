using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMove : MonoBehaviour
{
    public static event PlayerSpawn onPlayerSpawn;
    public delegate void PlayerSpawn(Transform t);
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool applyGravity;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float distToGrnd = 1.5f;
    public float gravityValue = -9.81f;
    private Vector3 move;
    public GameObject model;
    public Animator _anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        onPlayerSpawn.Invoke(transform);
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

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            _anim.SetBool("isRunning", true);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(move.x, 0f, move.z));
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, 40f * Time.deltaTime);
        } else 
        {
            _anim.SetBool("isRunning", false);
        }
    

   groundedPlayer = (Physics.Raycast(transform.position, Vector3.down, 1.1f, 1 << LayerMask.NameToLayer("Ground")));
 

        
        //groundedPlayer = controller.isGrounded;
        
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;  
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
    void PlayerAnimation(){
		_anim.SetBool ("isGrounded", controller.isGrounded);
		_anim.SetFloat ("runSpeed", (Mathf.Abs (Input.GetAxis ("Vertical"))+ Mathf.Abs(Input.GetAxis("Horizontal"))));
    }
    private bool IsGrounded()
    {
        float raycastDistance = controller.skinWidth + 0.1f;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(raycastOrigin, Vector3.down, raycastDistance);
    }
}
