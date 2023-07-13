using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject target;
    Camera cam;
    public float distance;
    [SerializeField] float fovZoom = 60f;
    [SerializeField] float zoomrate = 5f;

    
    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            transform.LookAt(target.transform.position, Vector3.up);
            if(transform.position.x != target.transform.position.x){
                transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
            }

            cam.fieldOfView -= zoomrate * .003f;
            distance += .003f;
        }
    }
    public IEnumerator ResetPosition()
    {
        for(int i = 0; i < 20; i++){
        yield return new WaitForSeconds(.01f);
        }
        distance = 0;
        float camFOV = cam.fieldOfView;
        cam.fieldOfView = fovZoom;
        yield return null;
    }
    void Start(){
        cam = GetComponentInChildren<Camera>();
    }
}
