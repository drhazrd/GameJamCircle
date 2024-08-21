using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public int damage = 10;
    public float radius = 3f;
    public float delay = 2f;
    float force = 400f;
    float fuseTimer;
    
    bool player;
    bool hasExploded;
    
    void Update()
    {
        Countdown();
    }

    private void Countdown()
    {
        fuseTimer -= Time.deltaTime;
        if (fuseTimer <= 0f && !hasExploded)
        {
            StartCoroutine(Explode());
            hasExploded = true;
            return;
        }
    }
    
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
           //Damage
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);

            }
        }
        //Audio Clip
        //VFX
        yield return new WaitForSeconds(.01f);
        Destroy(this.gameObject);
    }
}
