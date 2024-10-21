using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.right; // Axis to rotate around

    public float rotationSpeed = 10f; // Speed of rotation
    public float minRotation = 0f; // Minimum rotation angle
    public float maxRotation = 359f; // Maximum rotation angle

    public Light directionalLight; // Reference to the directional light

    public float sunriseTime = 6f; // Time of sunrise (in hours)
    public float sunsetTime = 18f; // Time of sunset (in hours)
    public float minIntensity = 0f; // Minimum light intensity
    public float maxIntensity = 1f; // Maximum light intensity

    private float currentRotation = 0f; // Current rotation angle

    [SerializeField]float timeOfDay;


    void Update()
    {
        // Rotate the object
        currentRotation += rotationSpeed * Time.deltaTime;
        if (currentRotation > maxRotation)
        {
            currentRotation = minRotation; // Reset rotation to loop
        }
        transform.rotation = Quaternion.Euler(rotationAxis * currentRotation);

        // Adjust light intensity based on the time of day
        timeOfDay = (currentRotation / 360f) * 24f; // Convert rotation to time of day in hours
        if (timeOfDay >= sunriseTime && timeOfDay <= sunsetTime)
        {
            float t = Mathf.InverseLerp(sunriseTime, sunsetTime, timeOfDay);
            directionalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
        else
        {
            float t = timeOfDay < sunriseTime ? Mathf.InverseLerp(0, sunriseTime, timeOfDay) : Mathf.InverseLerp(sunsetTime, 24, timeOfDay);
            directionalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }
    }
}
