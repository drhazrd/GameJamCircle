using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxAmmo;
    public static int currentAmmo;
    public float reloadTime;
    public Transform firePoint;
    public GameObject bullet;
    public float fireRate = 1f;
    public static bool isReloading;
}