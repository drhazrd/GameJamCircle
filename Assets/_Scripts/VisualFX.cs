using System;
using UnityEngine;


public class VisualFX : MonoBehaviour
{
    public static int playerPosition = Shader.PropertyToID("_Position");
    public static int playerSize = Shader.PropertyToID("_Size");

    public Material wallMaterial;
    public Camera Camera;
    public Transform gameCam;
    public LayerMask Mask;

    void Update()
    {
        var dir = Camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        if(Physics.Raycast(ray, 3000, Mask))
            wallMaterial.SetFloat(playerSize, 1);
        else
            wallMaterial.SetFloat(playerSize, 0);
        
        var veiw = Camera.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(playerPosition,veiw);
    }
}
