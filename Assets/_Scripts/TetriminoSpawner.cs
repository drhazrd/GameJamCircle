using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TetriminoSpawner : MonoBehaviour
{
    public static TetriminoSpawner gameManager;
    public GameObject[] tetrominoes;
    public TextMeshProUGUI scoreText;
    int score;


    void OnEnable(){}
    void Start()
    {
        CreateNewTetromino();
    }

    
    public void CreateNewTetromino()
    {
        int randompiece = Random.Range(0, tetrominoes.Length);
        Instantiate(tetrominoes[randompiece], transform.position, Quaternion.identity);
        score += 10;

    }
}
