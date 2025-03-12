using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private TestControls playerControls;
    Transform warpDestinationA, warpDestinationB;
    [SerializeField] bool playerHere;
    [SerializeField] bool canWarp;
    bool playerInA, playerInB;
    float warpTimer;
    Transform theTransported;
    public GameObject warpVFX;

    void Awake(){
        playerControls = new TestControls();
    }
    
    void Start(){
        playerControls.Player.Interact.performed += _ => WarpBody();
        warpDestinationA = transform.GetChild(0);
        warpDestinationB = transform.GetChild(1);
        warpDestinationA.gameObject.AddComponent<Teleport>();
        warpDestinationB.gameObject.AddComponent<Teleport>();
    }
    
    void OnEnable()
    {
        playerControls.Enable();
    }
    
    void OnDisable()
    {
        playerControls.Disable();
    }

    public void WarpBody()
    {
        CheckLocation();
        if(playerInA && canWarp){
            StartCoroutine(WarpObject(theTransported, warpDestinationB));
        } else if (playerInB && canWarp) {
            StartCoroutine(WarpObject(theTransported, warpDestinationA));
        }
    }

    private void CheckLocation()
    {
        playerInA = warpDestinationA.GetComponent<Teleport>().playerHere;
        playerInB = warpDestinationB.GetComponent<Teleport>().playerHere;
        playerHere = playerInA || playerInB;
        if(playerHere){
            theTransported = playerInA ? warpDestinationA.GetComponent<Teleport>().t : warpDestinationB.GetComponent<Teleport>().t;
        }
    }

    IEnumerator WarpObject(Transform t, Transform d){
        float time = .05f;
        Vector3 pos = t.position;
        CharacterController controller = transform.GetComponent<CharacterController>();
        
        if (warpVFX != null)Instantiate(warpVFX, transform.position, transform.rotation);
        if(controller != null)controller.enabled = false;
        GameObject _model = t.GetChild(0).gameObject;
        
        canWarp = false;
        _model.SetActive(false);
        t.position = d.position;
        Debug.Log($"Warping {t.gameObject.name} to {d.gameObject.name}");
        
        
        BombCopGameManager.Instance.PlayerFreeze();
        yield return new WaitForSeconds(.15f);
        
        if(BombCopUIManager.ui != null)BombCopUIManager.ui.ScreenFade(time);
        if (warpVFX != null)Instantiate(warpVFX, pos, transform.rotation);
        yield return new WaitForSeconds(.15f);
        _model.SetActive(true);
        if(controller != null)controller.enabled = true;
        
        BombCopGameManager.Instance.PlayerFreeze();
        canWarp = true;
    }
}
