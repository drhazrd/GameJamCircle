using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects camEffects;
    public AnimationCurve curve;
    public void Shake(float duration)
    {
        StartCoroutine(Shaking(duration));
    }
    IEnumerator Shaking(float dur){
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < dur) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/dur);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
    private void Awake()
    {
        if (camEffects != this && CameraEffects.camEffects != null)
        {
            Destroy(this);
        }
        else
        {
            camEffects = this;
        }
    }

}
