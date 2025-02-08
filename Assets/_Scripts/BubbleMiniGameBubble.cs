using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BubbleMiniGameBubble : MonoBehaviour
{
    public static event Action onBubblePop;
    Rigidbody2D my_rigidbody;
    CircleCollider2D my_collider;
    public GameObject popVFX;
    void Start()
    {
        my_collider = GetComponent<CircleCollider2D>();
        my_rigidbody = GetComponent<Rigidbody2D>();
        my_rigidbody.gravityScale = -.0025f;
    }
    public void Pop()
    {
        onBubblePop?.Invoke();
        Instantiate(popVFX, transform.position, transform.rotation);
        Destroy(gameObject); 
    }
}
