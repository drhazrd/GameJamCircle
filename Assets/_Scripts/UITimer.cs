using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    public TMP_Text display;
    public static float timer = 0f;
    public bool timerRun = true;

    void Start(){
        timer = 0f;
    }
    void Update(){
        if(GameManager.Instance.state == GameState.Play && timerRun)
            {
                timer += Time.deltaTime;
                display.text = "TIME: " + timer.ToString("F2");
            }
            
    }
}
