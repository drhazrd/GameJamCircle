using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoverTDS : MonoBehaviour
{
    public Camera sceneCamera;
    public GameObject aimObj;
    SlingShot weapon;
    
    public float speed = 2;
    
    Rigidbody2D rb;
    Vector2 movement;
    Vector2 aimPosition;

    [SerializeField] bool gamePad;
    [SerializeField] bool dashActive;
    [SerializeField] bool moveEnabled;
    PlayerControls controls;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => aimPosition = ctx.ReadValue<Vector2>();
        controls.Player.Aim.performed += ctx => aimPosition = ctx.ReadValue<Vector2>();
        controls.Player.Fire.performed += ctx => {
            if (weapon != null) weapon.Fire();
        };
        controls.Player.Action.performed += ctx => {
            if (!dashActive) Dash();
        };

        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<SlingShot>();
    }

    void Update()
    {
        bool aiming = Mathf.Abs(aimPosition.x) > 0.01f || Mathf.Abs(aimPosition.y) > 0.01f;
        if (aiming)
        {
            gamePad = false;
            aimPosition = sceneCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        else
        {
            gamePad = true;
            aimPosition = movement;
        }
    }
    
    void FixedUpdate()
    {
        if(moveEnabled) Move();
    }
    
    void Dash()
    {
        dashActive = true;
        StartCoroutine(DashTime());
    }

    IEnumerator DashTime()
    {
        speed *= 2;
        yield return new WaitForSeconds(0.3f);
        speed /= 2;
        yield return new WaitForSeconds(2f);
        dashActive = false;
    }
    
    void Move()
    {
        rb.velocity = movement * speed;
        Aim();
    }
    
    void Aim()
    {
        if (!gamePad)
        {
            Vector2 aimDirection = aimPosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            aimObj.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }
    }
}
