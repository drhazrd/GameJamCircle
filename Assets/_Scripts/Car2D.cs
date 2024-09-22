using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2D : MonoBehaviour
{
    [Range(0.1f, 0.95f)]
    public float driftFactor = 0.95f;

    [SerializeField]
    float accelerationPower = 30f;

    [SerializeField]
    float steeringPower = 3f;

    float steeringInput, accelerationInput;
    float rotationAngle;
    Rigidbody2D body;
    Vector2 inputVector;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        SetInputVector(inputVector);
    }
    void ApplyEngineForce(){
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationPower;
        body.AddForce(engineForceVector, ForceMode2D.Force);
    }
    void ApplySteering(){
        rotationAngle -= steeringInput * steeringPower;
        body.MoveRotation(rotationAngle);
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        KillOrthVelocity();
    
    }
    void KillOrthVelocity(){
        Vector2 forwardVelocity = transform.up * Vector2.Dot(body.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(body.velocity, transform.right);

        body.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
    public void SetInputVector(Vector2 carVector){
        steeringInput = carVector.x;
        accelerationInput = carVector.y;
    }
}
