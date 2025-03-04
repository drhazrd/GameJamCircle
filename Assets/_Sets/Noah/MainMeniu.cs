using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMeniu : MonoBehaviour
{
    public void StartGame(string level)
    {
        SceneManager.LoadScene(level);
    }
}
