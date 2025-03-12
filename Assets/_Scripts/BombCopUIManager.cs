using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BombCopUIManager : MonoBehaviour
{
    public static BombCopUIManager ui;
    public GameObject pauseMenuObject, interactObject, detonateObject, hudObject, timerObject, eventObject, statsObject;
    public TextMeshProUGUI gameDataUI, timerText;
    Image playerstat;
    int bombType, detonatorType;
    float timerTime;

    bool paused, timerActive, showTimer;
    TestControls controls;

    void Awake()
    {
        if(ui != this && ui==null){
            ui = this;
        } else{
            Destroy(this);
        }
        controls = new TestControls();
        playerstat = statsObject.GetComponentInChildren<Image>();


    }
    void OnEnable(){
		controls.Enable();
	}
	void OnDisable(){
		controls.Disable();
    }
    void Start()
    {
        controls.Player.Menu.performed += _ => Pause();
    }
    void Update()
    {
        paused = BombCopGameManager.Instance.paused;
        pauseMenuObject.SetActive(paused);
        timerActive = BombCopGameManager.Instance.state == GameState.Play;
        if(timerActive){
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        timerTime = BombCopGameManager.Instance.gameTime;
        timerText.text = timerTime.ToString("00");
    }

    public void Pause(){
        BombCopGameManager.Instance.PauseGame();
    }
    public void Quit(){
        BombCopGameManager.Instance.QuitGame();
    }
    void UpdatePlayerData(string data){
        gameDataUI.text = data;
    }
    public void UpdateUIData(int bombClassID, int bombDetonatorID, int bombTypeID, int bombStock, bool detonatorAvailable)
    {
        string[] bombTypeString = {"Tactical","Defensive","Powerful","Unpredictable","Wacky"};
        string[] detonatorString = {"Fuse","Remote","Link","Proximity"};
        if(detonateObject != null) detonateObject.SetActive(detonatorAvailable);
        string newGameData = $"{bombTypeString[bombTypeID]}\n {bombStock} | {detonatorString[bombDetonatorID]}";
        UpdatePlayerData(newGameData);
    }
    public void UpdatePlayerStats(float c, int m)
    {
        float amount =  c/m;
        if(playerstat != null){
            playerstat.fillAmount = amount;
            Debug.Log(amount);
        }
    }

    public void InteractButtonDisplay(ControlButton button, string interact2)
    {
        if(interactObject != null) interactObject.SetActive(true);
    }
    public void InteractButtonDisplay()
    {
        if(interactObject != null) interactObject.SetActive(true);
    }

    public void InteractButtonHide()
    {
        if(interactObject != null) interactObject.SetActive(false);
    }
    public void HUDDisplay()
    {
        if(hudObject != null) hudObject.SetActive(true);
    }

    public void HUDHide()
    {
        if(hudObject != null) hudObject.SetActive(false);
    }
    public void ShowTimer(){
        showTimer = !showTimer;
        if(timerObject != null) timerObject.SetActive(showTimer);

    }

    public void ScreenFade(float time)
    {
        if(eventObject != null){
            StartCoroutine(BlackOutFade(time));
        }
    }

    IEnumerator BlackOutFade(float length)
    {
        CanvasGroup canvasGroup = eventObject.GetComponentInChildren<CanvasGroup>();
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, length * .95f * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(length * .6f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, length * .95f * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator BlackOutOneWay(float length, bool fadeIn)
    {
        CanvasGroup canvasGroup = eventObject.GetComponentInChildren<CanvasGroup>();
        if(!fadeIn){
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, length * .95f * Time.deltaTime);
                yield return null;
            }
        } else {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, length * .95f * Time.deltaTime);
                yield return null;
            }
        }
   }

    public void FadeIn()
    {
        StartCoroutine(BlackOutOneWay(.5f, true));
    }
    public void FadeOut()
    {
        StartCoroutine(BlackOutOneWay(.5f, false));
    }
}
