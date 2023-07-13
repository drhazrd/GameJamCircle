using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject credits;
    void Start()
    {
        StartCoroutine(CreditsProcess());
    }
    IEnumerator CreditsProcess(){
        yield return new WaitForSeconds(25f);
        SceneManager.LoadScene(0);
    }
}
