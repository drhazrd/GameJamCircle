using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BombCopGameManager : MonoBehaviour
{
    public static BombCopGameManager Instance;
    public static event GameStarted onGameStart;
    public delegate void GameStarted();
    [SerializeField]public GameState state;
    Transform player;
    public static event Action<GameState> OnGameStateChanged;
    public bool paused {get; private set;}
    TestControls controls;
    public bool canMove {get; private set;}
    public float gameTime{get; private set;}


    void Awake()
    {
        Instance = this;
        controls = new TestControls();
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);
        StartGame();
        controls.Player.Menu.performed += _ => PauseGame();
    }

    public void PlayerFreeze(){
        canMove = !canMove;
    }
    void OnEnable(){
        controls.Enable();
    }
    void OnDisable(){
        controls.Disable();
    }
    private void StartGame()
    {
        UpdateGameState(GameState.Play);
        BombCopUIManager.ui.ShowTimer();

    }
    public void PauseGame(){
        paused = !paused;
        Time.timeScale = paused ? 0:1;
        if(paused){
            UpdateGameState(GameState.Pause);
        } else if (!paused){
            UpdateGameState(GameState.Play);
        }
        Debug.Log("TimeScale :" + Time.timeScale);
    }
    public void QuitGame(){
        //Save then Quit
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false; // Exits Play Mode
        #endif
    }

    public void UpdateGameState(GameState newState) 
    {
       state = newState;

       switch(newState)
       {
        case GameState.Play:
            Cursor.visible = false;
            canMove = true;
            break;  
        case GameState.Message:
            Cursor.visible = true;
            canMove = false;
            break;  
        case GameState.Pause:
            Cursor.visible = true;
            canMove = false;
            break;  
        case GameState.Lose:
            Cursor.visible = true;
            canMove = false;
            break;  
        case GameState.Win:
            Cursor.visible = true;
            canMove = false;
            break; 
        default:
            throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
       }
       OnGameStateChanged?.Invoke(newState);
    }
    void Update()
    {
        if(state == GameState.Play){
            gameTime += Time.deltaTime;
        }
    }
}
