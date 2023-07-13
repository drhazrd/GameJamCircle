
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event GameStarted onGameStart;
    public delegate void GameStarted();
    [SerializeField]public GameState state;
    Transform player;
    public static event Action<GameState> OnGameStateChanged;
    bool paused;
    PlayerControls controls;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartGame();
    }
    void OnEnable(){
        PlayMove2DLicht.onPlayerSpawn += PlayerSetup;
    }
    void OnDisable(){
        PlayMove2DLicht.onPlayerSpawn -= PlayerSetup;
    }
    void PlayerSetup(Transform transform) {
        player = transform;
    }
    void StartGame() {
        UpdateGameState(GameState.Play);
        onGameStart?.Invoke();
    }

    public void UpdateGameState(GameState newState) 
    {
       state = newState;

       switch(newState)
       {
        case GameState.Play:
            Cursor.visible = false;
            break;  
        case GameState.Pause:
            Cursor.visible = true;
            break;  
        case GameState.Lose:
            Cursor.visible = true;
            break;  
        case GameState.Win:
            Cursor.visible = true;
            break; 
        default:
            throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
       }
       OnGameStateChanged?.Invoke(newState);
    }
    
    public void PauseGame() {
        paused = !paused;
        //pauseCanvas
        if(paused) {
            Time.timeScale = 0.0f;
            UpdateGameState(GameState.Pause);
        } else {
            Time.timeScale = 1.0f;
            UpdateGameState(GameState.Play);
        }
    }


    public void Pause(InputAction.CallbackContext ctx) {
        if(ctx.performed)  {
            if(state == GameState.Play){
            }
            else if(state == GameState.Pause){
            }
        }
    }
    public void Win (){
        StopAllCoroutines();
        StartCoroutine(WinProcess());
    }

    IEnumerator WinProcess(){
        UpdateGameState(GameState.Win);
        yield return new WaitForSeconds(.25f);
        Time.timeScale = 0;
        //winCanvas
        Debug.LogWarning("Loading Next Level");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);

        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.buildIndex + 1);
        //Debug.LogWarning($"Reloading {scene} Level");
    }
    public void Lose (){
        StopAllCoroutines();
        StartCoroutine(LoseProcess());
    }
    IEnumerator LoseProcess(){
        UpdateGameState(GameState.Lose);
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        //loseCanvas
        Debug.LogWarning("Reloading Level");
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        Debug.LogWarning($"Reloading {scene} Level");
    }
   
}
 public enum GameState {
        Default, 
        Play, 
        Pause, 
        Lose, 
        Win
    }
