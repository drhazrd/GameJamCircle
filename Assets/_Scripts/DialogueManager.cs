using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueManager : MonoBehaviour
{
    public static event MessageOpenEvent onMessageOpen;
    public delegate void MessageOpenEvent ();
    public static event MessageCloseEvent onMessageClose;
    public delegate void MessageCloseEvent ();
    public static DialogueManager dialogue;
    public GameObject dialogueHolder;
    public TextMeshProUGUI textComponent, nameComponent;
    string sName;
    public string[] lines;

    [Range(0.01f, 0.5f)]
    public float textSpeed = .3f;
    TestControls testControls;
    int index;
    public bool dialogueActive{get; private set;}

    void Awake(){
        testControls = new TestControls();
        if (dialogue == null)
        {
            dialogue = this;
        }
        else 
        {
            Destroy(this);
        }
    }
    void Start()
    {
        testControls.Player.Interact.performed += _ => PlayerContuine();
        if(dialogueHolder.activeInHierarchy) dialogueHolder.SetActive(false);
        textComponent.text = string.Empty;
        nameComponent.text = string.Empty;
    } 
    void StartDialogue(){

        index = 0;
        dialogueActive = true;
        dialogueHolder.SetActive(dialogueActive);
        GameManager.gameManager.PlayerFreeze();
        StartCoroutine(TypeLine());
        UIManager.iManager.HUDHide();
    }
    IEnumerator TypeLine(){
        foreach (char c in lines[index].ToCharArray())
        {
            Debug.Log("Playing with Dialogue");
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        nameComponent.text = sName;
    }
    void NextLine(){
        if(index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            textComponent.text = string.Empty;
            nameComponent.text = string.Empty;
            dialogueActive = false;
            BombCopGameManager.Instance.PlayerFreeze();
            dialogueHolder.SetActive(dialogueActive);
            UIManager.iManager.HUDDisplay();
        }
    }
    public void PlayerContuine(){
        if(dialogueActive){
            if(textComponent.text == lines[index]){
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartMessage(string[] speakerLines, string speakerName, Sprite speakerImage)
    {
        lines = speakerLines;
        sName = speakerName;
        nameComponent.text = sName;
        //Sprite Image Update
        
        StartDialogue();
    }

}

[Serializable]
public class DialogueEntry {
    public string speakerName;
    public string[] lines;
    public AudioClip audioCue;
    public Sprite speakerImage;
}