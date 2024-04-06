using UnityEngine;

public class RiverControlPoint : MonoBehaviour
{
    public float width;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 GetPosition()
    {
        pos = transform.position;
        return transform.position;
    }
    public Quaternion GetRotation()
    {
        rot = transform.rotation;
        return transform.rotation;
    }
}