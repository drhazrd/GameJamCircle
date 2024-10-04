using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisManager : MonoBehaviour
{
    public static TetrisManager Instance { get; private set; }
    public int tetrominoCounter;
    public TetriminoSpawner spawner;
    public GameObject UI;

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
    }
    public void NewPlacement(){ 
        tetrominoCounter++;
        spawner.CreateNewTetromino();
    }

}
