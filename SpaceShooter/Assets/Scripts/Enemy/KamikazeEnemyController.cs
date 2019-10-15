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
    bool m_CanChangeDirection;

    void Start()
    {
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_MainCamera = Camera.main;
        m_CurrentState = EnemyStates.IDLE;
        m_MainShipTransform = m_MainShip.transform;
        m_CanChangeDirection = true;
    }

    void Update()
    {
        Vector3 viewPosition;
        switch (m_CurrentState)
        {
            case EnemyStates.IDLE:
                viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
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
                viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
                if ((viewPosition.y > 1.0f || viewPosition.y < 0.0f) && m_CanChangeDirection)
                {
                    m_Direction.y = -m_Direction.y;
                    m_CanChangeDirection = false;
                    Invoke("AllowChangeDirection", 0.25f);
                }
                m_SpaceshipBehaviour.Move(m_Direction.x, m_Direction.y);
            break;
            case EnemyStates.MOVE_TO_POINT:
                m_SpaceshipBehaviour.MoveTo(m_MainShipTransform.position);
            break;
        }
    }

    private void AllowChangeDirection()
    {
        m_CanChangeDirection = true;
    }
}
