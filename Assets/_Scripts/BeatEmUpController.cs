using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEmUpController : MonoBehaviour
{
    Animator animator;

    public int runSpeed = 1;
    public int walk = 2;
    public int run = 4;

    float horizontal;
    float vertical;
    bool faceingRight;

    int timesPressed = 0;

    bool isCrouching;
    
    float countSlide = 0;

    public float jumpForce = 300f;
    Rigidbody2D rb;
    float axisY;
    bool isJumping;

    bool isAttacking;

    void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.Sleep();
    }
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        if(Input.GetButton("Crouch") && (vertical == 0 && horizontal == 0)){
            isCrouching = true;
            animator.SetBool("isSliding", false);
            animator.SetBool("isCrouching", isCrouching);
        }
        else if(Input.GetButtonDown("Crouch") && horizontal != 0 && !isCrouching)
        {
            countSlide = 0.5f;
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("isSliding", true);
        }
        else if(Input.GetButtonUp("Crouch")){
            isCrouching = false;
            animator.SetBool("isCrouching", isCrouching);
        }

        if(countSlide > 0){
            animator.SetFloat("Speed", 0.0f);
            countSlide = countSlide - (1f * Time.deltaTime);
            if(countSlide <= 0){
                animator.SetBool("isSliding", false);
            }
        }
        if(Input.GetButtonUp("Horizontal")){
            timesPressed = 1;
        }
        if(Input.GetButtonUp("Horizontal") && timesPressed == 1){
            timesPressed = 2;
        }
        if(timesPressed == 2){
            runSpeed = run;
        }
        if(horizontal == 0 && vertical == 0){
            timesPressed = 0;
            runSpeed = walk;
        }
    }

    void FixedUpdate(){
        if(Input.GetButtonDown("Fire1")){
            isAttacking = true;
            if(vertical != 0 || horizontal != 0){
                vertical = 0;
                horizontal = 0;
                animator.SetFloat("Speed", 0);
            }
            animator.SetTrigger("Attack");
        }
        if(transform.position.y <= axisY) OnLanding();

        if(Input.GetButtonDown("Jump") && !isJumping){
            axisY = transform.position.y;
            isJumping = true;
            rb.gravityScale = 1.5f;
            rb.WakeUp();
            rb.AddForce(new Vector2(0, jumpForce));
            animator.SetBool("isJumping", isJumping);
        }
        if((horizontal != 0 || horizontal != 0) && !isCrouching && !isAttacking){
            Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }
        Flip(horizontal);
    }
    public void AlertObservers(string message){
        if(message == "AttackEnded"){
            isAttacking = false;
        }
    }
    void Flip(float h){
        if(horizontal < 0 && !faceingRight || horizontal > 0 && faceingRight){
            faceingRight = !faceingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void OnLanding(){
        isJumping = false;
        rb.gravityScale = 0f;
        rb.Sleep();
        axisY = transform.position.y;
        animator.SetBool("isJumping", false);
    }
}
