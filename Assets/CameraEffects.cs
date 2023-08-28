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
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < dur) {
            float strength = curve.Evaluate(elapsedTime/dur);
            
            float x = Random.Range(-1f,1f) * strength;
            float y = Random.Range(-1f,1f) * strength;

            transform.localPosition = new Vector3(x,y,startPosition.z);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        transform.localPosition = startPosition;
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
