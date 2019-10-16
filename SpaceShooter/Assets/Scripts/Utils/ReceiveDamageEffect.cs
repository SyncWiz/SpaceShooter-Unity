using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamageEffect : MonoBehaviour
{
    //Public 
    public Color m_DamageReceivedColor;
    public float m_DamageReceivedTime;
    [HideInInspector]
    public Color m_OriginalColor;

    //Private
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalColor = m_SpriteRenderer.color;
    }

    public void ApplyEffect()
    {
        m_SpriteRenderer.color = m_DamageReceivedColor;
        Invoke("RestoreOriginalColor", m_DamageReceivedTime);
    }

    void RestoreOriginalColor()
    {
        m_SpriteRenderer.color = m_OriginalColor;
    }
}
