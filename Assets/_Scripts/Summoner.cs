using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Gun
{
    private float nextTimeToSummon = 0f;

void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToSummon)
        {
            nextTimeToSummon = Time.time + 1f / fireRate;

            // Cast a ray from the camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Spawn your GameObject at the hit point
                Instantiate(bullet, hit.point, Quaternion.identity);
            }
        }
    }
}