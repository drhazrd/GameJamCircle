using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, groundDrag = 3f;
    float speedInput, turnInput;
    private bool grounded;
    public LayerMask groundLayer;
    public float groundRayLength = 0.5f;
    public Transform rayPoint;
    public Transform frontLeftWheel,frontRightWheel;
    public float maxWheelTurn;

    public ParticleSystem[] dustTrails;
    public float maxEmission = 25f;
    float emissionRate;

    void Start(){
        theRB.transform.parent = null;
    }
    
    void Update()
    {
        speedInput = 0f;
        if(Input.GetAxis("Vertical") > 0){
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
        } else if(Input.GetAxis("Vertical") < 0){
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");

        if(speedInput != 0  && grounded){
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (0f, turnInput * turnStrength * Time.deltaTime, 0f));
        }
        
        if(frontLeftWheel != null)frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, frontLeftWheel.localRotation.eulerAngles.z);
        if(frontRightWheel != null)frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, frontRightWheel.localRotation.eulerAngles.z);

        transform.position = theRB.transform.position;
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;
        if(Physics.Raycast(rayPoint.position, -transform.up, out hit, groundRayLength, groundLayer)){
            grounded = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        } 

        emissionRate = 0f;


        if(grounded){
            theRB.drag = groundDrag;
            if(Mathf.Abs(speedInput) > 0){
                theRB.AddForce(transform.forward * speedInput);

                emissionRate = maxEmission;
            }
        } else {
            theRB.drag = 0.1f;
            theRB.AddForce(Vector3.up * -gravityForce * 100f);
        }

        if(dustTrails.Length > 0)
        {
            foreach(ParticleSystem part in dustTrails)
            {
                var emissionModule = part.emission;
                emissionModule.rateOverTime = emissionRate;
            }
        }
    }
}