using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionChecker : MonoBehaviour
{
    int playerLevel, requiredLevel;
    public MissionObjectives mission;
    private DialogueManager messaging;

    ItemType checkerType;
    int checkerAmount;
    public int currentAmount;
    public static string missionGiverName;
    public string interactText;
    public string[] preLines, alterateLines, completedLines;
    public GameObject interactIcon;
    public GameObject _model;
    public bool canTalk, playerHere;
    Quaternion newRotation;
    Transform player;
    bool altLines, noLines;
    TestControls playerControls;
    float coolDown = 1.5f;


    void Start()
    {       
        noLines = preLines == null && alterateLines == null && completedLines == null; 
        if(!noLines){
            canTalk = true;
        }else{
            canTalk = false;
        }

        playerControls.Player.Interact.performed += _ => TriggerDialogue();
        messaging = DialogueManager.dialogue;
        if(_model==null){
            _model = transform.GetChild(0).gameObject;
        }
    }

    void OnEnable(){
		playerControls.Enable();
        DialogueManager.onMessageClose += PostCheckCooldown;

	}
	void OnDisable(){
		playerControls.Disable();
        DialogueManager.onMessageClose -= PostCheckCooldown;
	}

    private void OpenMission()
    {
        //Opens Mission Window
    }
    private void AcceptMission()
    {
        //Accepts Mission
        bool complete = true;
        if(complete){CompleteMission();}
    }
    private void CompleteMission()
    {
        //Checks if mission requirements have been met
    }
    private void CloseMission()
    {
        StartCoroutine(MissionGiverCooldown());
    }
    private void PostCheckCooldown()
    {
        if(playerHere){
            OpenMission();
        }
    }
    IEnumerator MissionGiverCooldown(){
        canTalk = false;
        yield return new WaitForSeconds(coolDown);
        canTalk = true;
    }
    

    void Update()
    {
        interactIcon.SetActive(playerHere);
        if (playerHere)
        {
            if(_model!=null){
                Vector3 playerLookingPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                _model.transform.LookAt(playerLookingPosition);
            }
        }
    }
    void TriggerDialogue()
    {
        if (playerHere && canTalk)
        {
            if (!messaging.dialogueActive)
            {
                if(!noLines) {
                    string[] lines = altLines ? alterateLines: completedLines;
                    messaging.StartMessage(lines, missionGiverName, null);
                }
            }
            else if (messaging.dialogueActive)
            {
                return;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            string interact = interactText != null ? "Interact": interactText;
            bool gamePad = GameManager.gameManager.isGamePad;
            playerHere = true;
            player = other.transform;
            if(!messaging.dialogueActive){
                UIManager.iManager.InteractButtonDisplay(interact); 
            }else {

                UIManager.iManager.InteractButtonHide();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIManager.iManager.InteractButtonHide();
            player = null;
            playerHere = false;
        }
    }
}