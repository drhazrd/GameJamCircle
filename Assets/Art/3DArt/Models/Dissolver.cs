using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private Material m_material;

    public bool dissolve;

    public float dissolveSpeed = 2f;
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D)){
            dissolve = !dissolve;
        }

        if(dissolve){
            m_material.SetFloat("_Noise_Strength", Mathf.MoveTowards(m_material.GetFloat("_Noise_Strength"), 1.25f, dissolveSpeed * Time.deltaTime));
        } else {
            m_material.SetFloat("_Noise_Strength", Mathf.MoveTowards(m_material.GetFloat("_Noise_Strength"), -.45f, dissolveSpeed * Time.deltaTime));
        }
    }
}
