using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrominoes : MonoBehaviour
{
    public static event Action onDeleteLine;
    public Vector3 rotationpoint;
    private float previousTime;
    public float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            transform.position += new Vector3(-1, 0, 0);
            if(!VaildMove()){
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            transform.position += new Vector3(1, 0, 0);
            if(!VaildMove()){
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        
        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime)){
            transform.position += new Vector3(0, -1, 0);
            if(!VaildMove()){
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                this.enabled = false;
                TetrisManager.Instance.NewPlacement();
                CheckForLines();
            }
            previousTime = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            transform.RotateAround(transform.TransformPoint(rotationpoint), new Vector3(0, 0, 1), 90);
            if(!VaildMove()){
                transform.RotateAround(transform.TransformPoint(rotationpoint), new Vector3(0, 0, 1), -90);
            }
        }
    }

    void CheckForLines(){
        //...
        for (int i = height - 1; i >= 0; i--)
        {
            if(HasLine(i)){
                DeleteLine(i);
                RowDown(i);
            }            
        }
    }
    
    void AddToGrid(){
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }
    bool HasLine(int i){
        for (int j = 0; j < width; j++)
        {
            if(grid[j,i] == null){
                return false;
            }
        }
        return true;
    }
    void DeleteLine(int i){
        //Add Lines and update score
        onDeleteLine?.Invoke();
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j,i] = null;
        }
    }
    void RowDown(int i){
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if(grid[j,y] != null){
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    bool VaildMove(){
        foreach (Transform children in transform){
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height){
                return false;
            }
            if(grid[roundedX, roundedY] != null){
                return false;
            }
        }
        return true;
    }
}
