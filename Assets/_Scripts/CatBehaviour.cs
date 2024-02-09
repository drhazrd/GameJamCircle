using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    public static event Action<CatBehaviour> onCatSpawn;
    [Header("Cat Stats")]
    public float aggression;
    public float hunger;
    public float thirst;
    public float cleanliness;
    public float stress;

    private float rateOfChange;

    [Header("Cat Data")]
    [SerializeField]int catID;
    public string catName;
    public float catAge;
    float speed;

    Vector2 movement;
    bool moving;
    bool canMove;
    Rigidbody2D rb;
    float coolDown;

    void Awake(){
    }
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        coolDown = UnityEngine.Random.Range(3f, 4f);
        speed = UnityEngine.Random.Range(15f, 20f);
        onCatSpawn?.Invoke(this);
        Setup();
    }

    void Update(){
        if(!moving){
            StartCoroutine(CatMover());
        }
    }

    IEnumerator CatMover(){
        moving = true;
        movement = new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f));
        rb.AddForce(movement * (speed + aggression));
        yield return new WaitForSeconds(coolDown); 
        moving = false;
        yield return new WaitForSeconds(coolDown); 

    }

    void Setup(){

    }
}
