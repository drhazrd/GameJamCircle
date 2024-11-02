using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoverTDS : MonoBehaviour
{
    public Camera sceneCamera;
    public GameObject aimObj;
    SlingShot weapon;
    
    public float speed = 2;
    
    Rigidbody2D rb;
    Vector2 movement;
    Vector2 aimPosition;

    [SerializeField]bool gamePad;
    [SerializeField]bool dashActive;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<SlingShot>();
    }

    void Update()
    {
        ProcessInputs();
        if(Input.GetButtonDown("Jump")){
            if(!dashActive) Dash(); else return;
        }
        if(Input.GetButtonDown("Fire1")){
            if(weapon!=null) weapon.Fire(); else return;
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        if (Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveY) > 0.01f)
        {
            gamePad = false;
            aimPosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            gamePad = true;
            aimPosition = new Vector2(Input.GetAxisRaw("GamepadHorizontal"), Input.GetAxisRaw("GamepadVertical"));
        }
    }
    void Dash(){
        dashActive = true;
        StartCoroutine(DashTime());
    }
    IEnumerator DashTime(){
        speed *=2;
        yield return new WaitForSeconds(.3f);
        speed /=2;
        yield return new WaitForSeconds(2f);
        dashActive = false;
    }
    void Move(){
        rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
        Aim();
    }
    void Aim()
    {
        if(!gamePad){
            Vector2 aimDirection = aimPosition - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            aimObj.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        }
    }
}
