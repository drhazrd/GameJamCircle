using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject splashMenu, mainMenu, optionsMenu, creditsMenu;
    public float fastestTime;
    public int deathCount;
    public int winCount;
    public int highScore;

    void Start()
    {
        LoadPlayerData();
        FullMenuReset();
    }

    void Update()
    {
        
    }

    public void MainMenuToggle()
    {
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
    }

    public void CreditMenuToggle()
    {
        creditsMenu.SetActive(!creditsMenu.activeInHierarchy);
    }

    public void OptionsMenuToggle()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
    }

    void FullMenuReset()
    {
        splashMenu.SetActive(true);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void DismissSplashScreen()
    {
        splashMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StartGame");
    }

    public void QuitGame()
    {
        SavePlayerData();
        Application.Quit();
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetFloat("FastestTime", fastestTime);
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.SetInt("WinCount", winCount);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    void LoadPlayerData()
    {
        fastestTime = PlayerPrefs.GetInt("FastestTime", 0);
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        winCount = PlayerPrefs.GetInt("WinCount", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}