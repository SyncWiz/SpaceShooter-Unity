using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    //Public
    public Vector2 m_Direction;
    public float m_Speed;

    //Private
    private Rigidbody2D m_Rigidbody2d;
    private Renderer m_Renderer;
    private Camera m_MainCamera;

    void Start()
    {
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<Renderer>();
        m_MainCamera = Camera.main;

        m_Rigidbody2d.gravityScale = 0.0f;
        m_Rigidbody2d.velocity = m_Direction * m_Speed;
    }

    void Update()
    {
        CheckForDestroy();
    }

    void CheckForDestroy()
    {
        Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
        if (viewPosition.x > 1.0f || viewPosition.x < 0.0f || viewPosition.y > 1.0f || viewPosition.y < 0.0f)
        {
            Destroy(gameObject);
        }
        Debug.Log(viewPosition);
    }
}
