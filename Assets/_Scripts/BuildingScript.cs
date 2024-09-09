using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public Material[] m_materials;
    Renderer m_renderer;
    int materialID;
    void Start()
    {
        materialID = Random.Range(0, m_materials.Length);
        m_renderer = GetComponent<Renderer>();
        if(m_materials != null) m_renderer.material = m_materials[materialID];
    }

        

    void UpdateColor()
    {
        
    }
}
