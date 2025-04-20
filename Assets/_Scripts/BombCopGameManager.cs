using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class BombCopGameManager : GameManager
{
    public static BombCopGameManager Instance;

    Transform player;
    TestControls controls;
    private bool activeTimer;

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
    
    void OnEnable(){
        controls.Enable();
    }
    void OnDisable(){
        controls.Disable();
    }
    private void StartGame()
    {
        UpdateGameState(GameState.Play);
        base.SetGameTime(60f);
        ToggleTimer();

        if(BombCopUIManager.ui != null){
            BombCopUIManager.ui.ShowTimer();
            BombCopUIManager.ui.FadeIn();
        }

    }
    public void ToggleTimer(){
        activeTimer = !activeTimer;
        return;
    }
    
    public void Pause(){
        PauseGame();
    }
    public void QuitGame(){
        Quit();
    }
    
}
