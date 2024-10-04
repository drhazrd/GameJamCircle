using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoSpawner : MonoBehaviour
{
    public GameObject[] tetrominoes;


    void OnEnable(){}
    void Start()
    {
        CreateNewTetromino();
    }

    
    public void CreateNewTetromino()
    {
        Instantiate(tetrominoes[Random.Range(0, tetrominoes.Length)], transform.position, Quaternion.identity);
    }
}
