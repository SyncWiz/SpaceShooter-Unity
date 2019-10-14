using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //Public
    public Vector3 m_Direction;
    public float m_Speed;
    public int m_Damage;
    public bool m_AllyBullet;

    //Private
    private Camera m_MainCamera;
    private string m_TagDetection;

    void Start()
    {
        m_MainCamera = Camera.main;
        if(m_AllyBullet)
        {
            m_TagDetection = "Enemy";
        }
        else
        {
            m_TagDetection = "MainShip";
        }
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //TODO update this in the future
        if (collider.gameObject.tag == m_TagDetection)
        {
            Destroy(gameObject);
        }
    }
}
