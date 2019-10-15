using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    //Public 
    public float m_Degrees;
    public float m_Health;
    public float m_Speed;
    public bool m_IsMain;
    public int m_NumberOfChunksAfterExplosion;
    public Vector3 m_RotationDirection;
    [HideInInspector]
    public Vector3 m_Direction;
    public GameObject m_Chunk;

    //Private
    AsteroidBehaviour m_AsteroidBehaviour;
    ReceiveDamageEffect m_ReceiveDamageEffect;

    void Start()
    {
        m_ReceiveDamageEffect = GetComponent<ReceiveDamageEffect>();
        if (m_IsMain)
        {
            m_AsteroidBehaviour = m_Chunk.GetComponent<AsteroidBehaviour>();
        }
    }

    void Update()
    {
        if(!m_IsMain)
        {
            Move();
        }
        Rotate();
        CheckHealth();
        if (!MathUtils.IsPointInsideCameraView(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.position += m_Direction * m_Speed * Time.deltaTime;
    }

    void Rotate()
    {
        Vector3 rotation = m_RotationDirection * m_Degrees * Time.deltaTime;
        transform.Rotate(rotation);
    }

    void CheckHealth()
    {
        if (m_Health <= 0)
        {
            Explode();
        }
    }

    void ReceiveDamage()
    {
        m_Health -= 1;
        m_ReceiveDamageEffect.ApplyEffect();
    }

    void Explode()
    {
        if(m_IsMain)
        {
            Vector3 center = transform.position;
            for (int i = 0; i < m_NumberOfChunksAfterExplosion; i++)
            {
                Vector3 position = MathUtils.GetRandomPositionAtCircle(center, 0.5f);
                Vector3 direction = position - transform.position;
                direction.z = 0;
                direction.Normalize();
                m_AsteroidBehaviour.m_Direction = direction;
                Instantiate(m_Chunk, position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
        //TODO sound
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainShipBullet")
        {
            ReceiveDamage();
        }
        if (collider.gameObject.tag == "MainShipMissile")
        {
            Explode();
        }
    }
}
