using UnityEngine;
public class ColorChanger : MonoBehaviour
{
    public MeshRenderer cubeRenderer;

    private void Start()
    {
        if (cubeRenderer == null)
        {
            // Assuming the cube has a MeshRenderer component attached to it
            cubeRenderer = GetComponent<MeshRenderer>();
        }

        if (cubeRenderer != null)
        {
            // Generate a random color
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // Assign the random color to the cube's material
            cubeRenderer.material.color = randomColor;
        }
    }
}