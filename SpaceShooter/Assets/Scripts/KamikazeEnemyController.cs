using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyController : MonoBehaviour
{
    public enum KamikazeType
    {
        UNIFORM,
        STATIC,
        CHASER
    }

    //public
    public Vector3 m_Direction;
    public KamikazeType m_ShipType;
    public GameObject m_MainShip;

    //private
    SpaceshipBehaviour m_SpaceshipBehaviour;
    Camera m_MainCamera;
    EnemyStates m_CurrentState;
    Transform m_MainShipTransform;

    void Start()
    {
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_MainCamera = Camera.main;
        m_CurrentState = EnemyStates.IDLE;
        m_MainShipTransform = m_MainShip.transform;
    }

    void Update()
    {
        Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
        switch (m_CurrentState)
        {
            case EnemyStates.IDLE:
                
                if (viewPosition.x <= 1.0f && viewPosition.x >= 0.0f && viewPosition.y <= 1.0f && viewPosition.y >= 0.0f)
                {
                    if (m_ShipType == KamikazeType.STATIC)
                    {
                        m_CurrentState = EnemyStates.WAIT;
                    }
                    else
                    {
                        if(m_ShipType == KamikazeType.UNIFORM)
                        {
                            m_CurrentState = EnemyStates.MOVE;
                        }
                        else
                        {
                            m_CurrentState = EnemyStates.MOVE_TO_POINT;
                        }
                        
                    }
                }
            break;
            case EnemyStates.MOVE:
                if (viewPosition.y >= 1.0f || viewPosition.y <= 0.0f)
                {
                    m_Direction.y = -m_Direction.y;
                }
                m_SpaceshipBehaviour.Move(m_Direction.x, m_Direction.y);
            break;
            case EnemyStates.MOVE_TO_POINT:
                m_SpaceshipBehaviour.MoveTo(m_MainShipTransform.position);
            break;
        }
        if (viewPosition.x <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainShipBullet")
        {
            m_SpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<BulletBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "MainShipMissile")
        {
            m_SpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<MissileBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "MainShipExplosion")
        {
            //TODO think how to do this
            m_SpaceshipBehaviour.ReceiveDamage(1);
        }
    }
}
