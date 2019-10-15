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
        if(!MathUtils.IsPointInsideCameraView(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.position += m_Direction * m_Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == m_TagDetection)
        {
            if(m_AllyBullet)
            {
                Destroy(gameObject);
            }
            else if(collider.gameObject.GetComponent<SpaceshipBehaviour>().m_CanRecieveDamage)
            {
                Destroy(gameObject);
            }
        }

        if(collider.gameObject.tag == "Asteroid")
        {
            if (m_AllyBullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
