using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGoal : MonoBehaviour
{
    void OnTriggerEnter()
    {
        GameManager.gameManager.Win();
    }
}
