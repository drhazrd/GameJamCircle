using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    
    public void LoadLevel(string name)
    {
        Debug.Log("Loading: " + name);
        SceneManager.LoadScene(name);
    }
    public void ReloadLevel()
    {
        Debug.Log("Reload Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
    }

    public void Resume()
    {
        GameManager.Instance.UpdateGameState(GameState.Play);
    }

    public void LoadNextLevel()
    {
        // load the nextlevel
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
 
    }
    public void DisableCanvas(GameObject disable)
    {
        //disable.SetActive(false);
        disable.GetComponent<Canvas>().enabled = false;
        
    }
    public void EnableCanvas(GameObject enable)
    {
        //enable.SetActive(true);
        enable.GetComponent<Canvas>().enabled = true;
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
}

