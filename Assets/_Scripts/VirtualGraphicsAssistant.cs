using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualGraphicsAssistant : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer m_renderer;

    void Awake(){m_renderer = GetComponent<SpriteRenderer>();}

    public void AssignPetGraphics(PetType t){
        switch (t){
            case PetType.blueWhale:
                m_renderer.sprite = sprites[0];
                break;
            case PetType.ghost:
                m_renderer.sprite = sprites[1];
                break;
            case PetType.narwhal:
                m_renderer.sprite = sprites[2];
                break;
            case PetType.owl:
                m_renderer.sprite = sprites[3];
                break;
            case PetType.penguin:
                m_renderer.sprite = sprites[4];
                break;
        }
    }
}
