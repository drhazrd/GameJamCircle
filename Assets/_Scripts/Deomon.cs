using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deomon : MonoBehaviour
{
    public Transform target;
    public float range = 15f;
    public float turnSpeed = 15f;
    public float projectileSpeed = 7f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;
    public GameObject projectile;
    public Transform[] firePoints;
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget(){
        GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            if(nearestEnemy != null && shortestDistance <= range){
                target = nearestEnemy.transform;
            } else {
                target = null;
            }
        }

    }
    void Update()
    {
        if(target == null)
            return;
    
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);   
        
        if(fireCountdown <= 0f){
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        if(fireCountdown > 0) fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        if(projectile != null){
            for (int i = 0; i < firePoints.Length; i++)
            {
                GameObject newProjectile = Instantiate(projectile, firePoints[i].position, firePoints[i].rotation) as GameObject;
                Rocket rocket = newProjectile.GetComponent<Rocket>();
                if(rocket != null){
                    rocket.Seek(target);
                    rocket.speed = projectileSpeed;
                }
            }
        } 
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
