using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{   
    //Public
    public Vector3 m_Direction;
    public float m_Speed;

    //Private
    private Camera m_MainCamera;

    void Start()
    {
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        Move();
        CheckForDestroy();
    }

    void Move()
    {
        transform.position += m_Direction * m_Speed * Time.deltaTime;
    }

    void CheckForDestroy()
    {
        Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
        if (viewPosition.x > 1.0f || viewPosition.x < 0.0f || viewPosition.y > 1.0f || viewPosition.y < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
