using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRadius = 3f; // Radius within which the player can interact
    public KeyCode interactionKey = KeyCode.E; // The key to trigger interaction (you can change this)

    [Header("Events")]
    public UnityEvent onInteract; // UnityEvent to handle interactions
    private DefaultInputActions inputActionsMap;
    public GameObject thing;
    public Transform player; // Reference to the player's transform
    bool playerHere;
    
    private void OnEnable(){
        inputActionsMap.Enable();
    }
    
    private void OnDisable(){
        inputActionsMap.Disable();
    }

    private void Start(){

    }
    private void Update()
    {
        // Calculate the distance between this object and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the interaction radius
        if (distanceToPlayer <= interactionRadius)
        {
            playerHere = true;
            // Check if the interaction key is pressed
            if (Input.GetKeyDown(interactionKey))
            {
                // Trigger the interaction event
                Activate();
            }
        } else playerHere = false;
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerHere = false;
            player = null;
            thing.SetActive(false);
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerHere = true;
            player = other.transform;
            thing.SetActive(true);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    void Activate(){
        if(playerHere && onInteract != null) onInteract.Invoke();
    }
}