using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class RiverControlSystem : MonoBehaviour
{
    public List<RiverControlPoint> controlPoints = new List<RiverControlPoint>();
    public Material riverMaterial;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

#if UNITY_EDITOR
    void OnValidate()
    {
        GenerateRiverMesh();
    }
#endif

    public void GenerateRiverMesh()
    {
        if (controlPoints.Count < 2)
        {
            Debug.LogWarning("River needs at least 2 control points to generate.");
            return;
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        Vector3 previousPoint = controlPoints[0].GetPosition();
        Quaternion previousRotation = controlPoints[0].GetRotation();

        for (int i = 1; i < controlPoints.Count; i++)
        {
            Vector3 currentPoint = controlPoints[i].GetPosition();
            Quaternion currentRotation = controlPoints[i].GetRotation();

            // Calculate river width and direction based on control points
            // Update vertices and triangles accordingly

            previousPoint = currentPoint;
            previousRotation = currentRotation;
        }

        // Generate mesh based on vertices and triangles

        // Ensure meshFilter and meshRenderer components are set
        SetupMeshComponents();
    }

    private void SetupMeshComponents()
    {
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            
        }
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = riverMaterial;
        }

        meshFilter.mesh = new Mesh();  // Create a new mesh instance
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Display control points in the scene view for easier editing
        foreach (var point in controlPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(point.GetPosition(), 0.1f);
        }
    }
#endif
}
