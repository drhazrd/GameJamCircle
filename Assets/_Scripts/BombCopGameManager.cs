using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    private bool activeTimer;

    public bool canMove {get; private set;}
    public float gameTime{get; private set;}
    public Transform spawnLocation;

    private MinimapCamera miniCam;
    private CinemachineVirtualCamera gameCamera;

    public GameObject playerCharacter;


    void Awake()
    {
        Instance = this;
        controls = new TestControls();
        gameCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    IEnumerator Start()
    {
        controls.Player.Menu.performed += _ => PauseGame();
        CreateMiniMapCamera();
        if(playerCharacter != null) SpawnCharacter();
        yield return new WaitForSeconds(4f);
        StartGame();
    }

    private void SpawnCharacter()
    {
        GameObject newPlayer = Instantiate(playerCharacter, spawnLocation.position, spawnLocation.rotation) as GameObject;
        RegisterPlayer(newPlayer.transform);
    }

    private void CreateMiniMapCamera()
    {
        GameObject g = new GameObject();
        GameObject camObject = Instantiate(g, new Vector3(0, 10f, 0), Quaternion.identity) as GameObject;
        camObject.AddComponent<Camera>();
        camObject.AddComponent<MinimapCamera>();
        miniCam = camObject.GetComponent<MinimapCamera>();
    }
    public void RegisterPlayer(Transform t){
        player = t;
        ChangeCamTarget(player);
    }
    public void ChangeCamTarget(Transform newTarget){
        AssignCamTarget(newTarget);
    }
    void AssignCamTarget(Transform newTarget)
    {
        miniCam.TargetSetup(newTarget);
        gameCamera.LookAt = newTarget;
        gameCamera.Follow = newTarget;
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
        gameTime = 60f;
        ToggleTimer();
        BombCopUIManager.ui.ShowTimer();
        BombCopUIManager.ui.FadeIn();

    }
    public void ToggleTimer(){
        activeTimer = !activeTimer;
        return;
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
            ToggleTimer();
            BombCopUIManager.ui.ShowTimer();
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
        if(activeTimer){
            gameTime -= Time.deltaTime;
        } else {
            gameTime = 0;
            return; 
        }
        if(gameTime == 0){
            UpdateGameState(GameState.Lose);
            return;
        }
    }
    
}
