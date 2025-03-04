using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    Transform player;
    TestControls playerControls;
    public DialogueEntry dialogueEntry;
    DialogueManager dialogueManager;

    public bool canTalk, playerHere;
    float coolDown = 1.5f;
    private GameObject interactIcon;
    private GameObject _model;
    string interactText;
    public string[] lines;
    private object npcName;

    void Awake () {
        playerControls = new TestControls();
    }
    void OnEnable(){
		playerControls.Enable();
	}
	void OnDisable(){
		playerControls.Disable();
	}
    void Start()
    {
        if(lines.Length > 0){
            dialogueEntry.lines = lines;
        }
        if(dialogueEntry.lines !=null){
            canTalk = true;
        }else{
            canTalk = false;
        }
        dialogueManager = DialogueManager.dialogue;
        playerControls.Player.Interact.performed += _ => TriggerDialogue();

    }

    void Update()
    {
        if(interactIcon != null) interactIcon.SetActive(playerHere);
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
            player.GetComponent<BomberPlayerController>().FaceDirection(transform.position);
            if (!dialogueManager.dialogueActive)
            {
                string npcName = this.gameObject.name;
                if(lines != null) dialogueManager.StartMessage(lines, npcName, null);
            }
            else if (dialogueManager.dialogueActive)
            {
                dialogueManager.PlayerContuine();
                return;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            string interact = interactText != null ? "Interact": interactText;
            bool gamePad = other.gameObject.GetComponent<BomberPlayerController>().isGamePad;
            playerHere = true;
            player = other.transform;
            if(!dialogueManager.dialogueActive){
                BombCopUIManager.ui.InteractButtonDisplay(); 
            }else {

                BombCopUIManager.ui.InteractButtonHide();
            }
            //if(_anim != null) _anim.SetBool("playerHere",playerHere);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BombCopUIManager.ui.InteractButtonHide();
            player = null;
            playerHere = false;
        }
    }
    void PostDialogueCooldown(){
        if(playerHere){
            StartCoroutine(DialogueCooldown());
        }
    }
    IEnumerator DialogueCooldown(){
        canTalk = false;
        yield return new WaitForSeconds(coolDown);
        canTalk = true;
    }
}
