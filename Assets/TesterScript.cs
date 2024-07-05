using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterScript : MonoBehaviour
{
    public float radius = 2f; // Distance from the center
    public float speed = 1f; // Speed of rotation
    float timer = 0; // Speed of rotation
    public bool canFire;
    public BulletController fireProjectile;
    private void Update(){
        if(canFire){
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                if(fireProjectile != null) {
                    BulletController bullet = Instantiate(fireProjectile, child.position, child.rotation);
                    bullet.speed = 5;
                }

            }
            timer = 5f;
        }
        canFire = timer <= 0;
        if(timer > 0){
            timer -= Time.deltaTime; 
        } else {
            return;
        }    

    }
    private void Start()
    {
        MoveChildrenInCircle();
    }

    void MoveChildrenInCircle()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float angle = (i / (float)transform.childCount) * 2 * Mathf.PI;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            newPos += transform.position; // Center point of the parent object
            child.position = Vector3.MoveTowards(child.position, newPos, 100f * Time.deltaTime);
            child.rotation = Quaternion.Euler(new Vector3(0, (360f/transform.childCount) * i + (360f / transform.childCount), 0));
        }
    }
}
