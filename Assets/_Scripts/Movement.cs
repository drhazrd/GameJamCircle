using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public GameObject playerCam;
    public Camera gameCamera;
    public GameObject player;
    public Transform playerTransform;
    public Transform gameCameraTransform;
    public InputAction playerMovement;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector2 mouseLook;
    [Header("Numbers")]
    public bool IsGrounded;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float mouseSens = 100f;
    public float xRotation = 0f;
    public float FOV = 60f; 

    private void OnEnable()
    {
        playerMovement.Enable();
    }
    
    private void OnDisable()
    {
        playerMovement.Disable();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void look()
    {
        gameCameraTransform = playerCam.transform;
        playerTransform = player.transform;
        Vector2 mouseXY = Mouse.current.delta.ReadValue() * (mouseSens * Time.deltaTime);
        xRotation -= mouseXY.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        gameCameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerTransform.Rotate(Vector3.up * mouseXY.x);
    }
    
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        look();
        Camera.SetupCurrent(gameCamera);
        
        IsGrounded = controller.isGrounded;
        if (IsGrounded && playerVelocity.y <= 0)
        {
            playerVelocity.y = -1f;
        }
        
        var movement = playerMovement.ReadValue<Vector3>();
        Vector3 move = ((movement.z * playerTransform.forward)  + (movement.x * playerTransform.right));
        controller.Move(move * Time.deltaTime * playerSpeed);
        bool spaceKeyPressed = Keyboard.current.spaceKey.isPressed;
        
       if (spaceKeyPressed && IsGrounded)
       {
           playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
       }
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
