using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public Color m_color;
    Renderer m_renderer;
    float r,b,g,a;
    void Start()
    {
        r = UnityEngine.Random.Range(0f, 255f);
        b = UnityEngine.Random.Range(0f, 255f);
        g = UnityEngine.Random.Range(0f, 255f);
        a = 1;
        UpdateColor();
    }

    // Update is called once per frame
    void UpdateColor()
    {
        m_color = new Color(r,b,g);
        m_color.a = 1f;
        m_renderer = GetComponent<Renderer>();
        m_renderer.material.color = m_color;
        
    }
}
