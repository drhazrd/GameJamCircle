using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TetrisManager : MonoBehaviour
{
    public static TetrisManager Instance { get; private set; }
    public int tetrominoCounter, scoreCounter, lineCount;
    public TetriminoSpawner spawner;
    public GameObject UI;
    public TextMeshProUGUI score, lines;
    public Transform holder, newSpawnLocation;

    public bool gameOver{get; private set;}


    void OnEnable(){
        Tetrominoes.onDeleteLine += NewLine;
    }
    void OnDisable(){
        Tetrominoes.onDeleteLine -= NewLine;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start(){
        tetrominoCounter = 0;
        gameOver = false;
    }
    public void NewPlacement(){ 
        tetrominoCounter++;
        if(!gameOver) spawner.CreateNewTetromino();
        UpdateScore();
    }
    public void NewLine(){ 
        lineCount ++;
        lines.text = $"{lineCount}";
        scoreCounter += 800;
    }
    public void UpdateScore(){ 
        scoreCounter += 10;
        score.text = $"{scoreCounter}";
    }
    public void GameOver(){
        gameOver = !gameOver;
    }

}
