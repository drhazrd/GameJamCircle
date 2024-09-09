using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerddSlingShot : MonoBehaviour
{
    public LineRenderer [] lineRenderers;
    public Transform[] stripPosition;
    public Transform center;
    public Transform idelPosition;
    public Vector3 currentPosition;
    public GameObject berrdPrefab;
    Rigidbody2D berrd;
    Collider2D berrdCollider;
    public float berrdPositionOffset;
    [Range(1f, 5f)]
    public float maxLength = 3f;
    public float bottomBoundry;

    bool isMouseDown;
    public float force;

    void Start(){
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPosition[0].position);
        lineRenderers[1].SetPosition(0, stripPosition[1].position);
        if(berrdPrefab != null){
            CreateBird();
        }
    }
    void Update(){
        if(isMouseDown){
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 5;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLength);
            currentPosition = ClampBoundry(currentPosition);
            SetStrips(currentPosition);
            if(berrdCollider) berrdCollider.enabled = true;
        } else {
            ResetStrips();
        }
    }
    void CreateBird(){
        berrd = Instantiate(berrdPrefab).GetComponent<Rigidbody2D>();
        berrdCollider = berrd.GetComponent<Collider2D>();
        berrdCollider.enabled = false;
        berrd.isKinematic = true;

    }
    void OnMouseDown(){
        isMouseDown = true;
    }
    void OnMouseUp(){
        isMouseDown = false;
        Shoot();
    }
    void Shoot(){
        
        berrd.isKinematic = false;
        Vector3 berrdForce = (currentPosition - center.position) * force * -1;
        berrd.velocity = berrdForce;
        Destroy(berrd.gameObject, 15f);

        berrd = null;
        berrdCollider = null;
        
        Invoke("CreateBird", 2);
    }
    void ResetStrips(){
        currentPosition = idelPosition.position;
        SetStrips(currentPosition);
    }
    Vector3 ClampBoundry(Vector3 clampVector){
        clampVector.y = Mathf.Clamp(clampVector.y, bottomBoundry, 1000);
        return clampVector;
    }
    void SetStrips(Vector3 position){
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);
        if(berrd){   
            Vector3 dir = position - center.position;
            berrd.transform.position = position + dir.normalized * berrdPositionOffset;
            berrd.transform.right = -dir.normalized;
        }
    }
}
