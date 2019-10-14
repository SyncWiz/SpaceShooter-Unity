using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemyController : MonoBehaviour
{
    enum States
    {
        IDLE,
        IMPULSE,
        WAIT,
        WAIT_SHOOT
    }
    //Public
    public float m_DistanceToStartBehaving;
    public float m_CenterDestinationPositionOffset;
    public float m_DistanceAccelerating;
    public float m_DistanceToChangeDestination;
    public float m_ShootTime;
    public float m_CameraLimitYOffset;
    public GameObject m_MainShip;
    public bool m_InvertHalf;
    public bool m_IsStatic;

    //Private
    private States m_CurrentState;
    private SpaceshipBehaviour m_SpaceshipBehaviour;
    private Camera m_MainCamera;
    private Vector3 m_TargetPosition;
    private float m_CurrentTime;

    void Start()
    {
        m_CurrentState = States.IDLE;
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_MainCamera = Camera.main;
        GenerateTargetPosition();
    }

    void Update()
    {
        CheckShoot();
        UpdateTimings();
    }

    void FixedUpdate()
    {
        switch(m_CurrentState)
        {
            case States.IDLE:
                if (Vector3.Distance(m_MainShip.transform.position, transform.position) <= m_DistanceToStartBehaving)
                {
                    if(!m_IsStatic)
                    {
                        m_CurrentState = States.IMPULSE;
                    }
                    else
                    {
                        m_CurrentState = States.WAIT_SHOOT;
                    }
                }
            break;
            case States.IMPULSE:
                Vector2 force = new Vector2(0, m_TargetPosition.y);
                m_SpaceshipBehaviour.MoveForce(force);
                if (Vector3.Distance(m_TargetPosition, transform.position) <= m_DistanceAccelerating)
                {
                    GenerateTargetPosition();
                }
            break;
            case States.WAIT:
                if (Vector3.Distance(m_TargetPosition, transform.position) <= m_DistanceToChangeDestination)
                {

                    m_CurrentState = States.IMPULSE;
                }
                break;
        }

        if (Debug.isDebugBuild)
        {
            if (Vector3.Distance(m_MainShip.transform.position, transform.position) >= m_DistanceToStartBehaving)
            {
                Debug.DrawLine(m_MainShip.transform.position, transform.position, Color.red);
            }
            else
            {
                Debug.DrawLine(m_MainShip.transform.position, transform.position, Color.green);
            }
            Debug.DrawLine(transform.position, m_TargetPosition, Color.yellow);
        }
    }

    void GenerateTargetPosition()
    {
        float cameraPositionY = m_MainCamera.gameObject.transform.position.y;
        float randomY;
        if (m_InvertHalf)
        {
            randomY = Random.Range(cameraPositionY + m_CenterDestinationPositionOffset, cameraPositionY + (m_MainCamera.orthographicSize- m_CameraLimitYOffset));
        }
        else
        {
            randomY = Random.Range(cameraPositionY - m_CenterDestinationPositionOffset, cameraPositionY - (m_MainCamera.orthographicSize- m_CameraLimitYOffset));
        }
        m_InvertHalf = !m_InvertHalf;
        m_TargetPosition = new Vector3(transform.position.x, randomY, transform.position.z);
    }

    void UpdateTimings()
    {
        m_CurrentTime += Time.deltaTime;
    }

    void CheckShoot()
    {
        if(m_CurrentTime >= m_ShootTime && m_CurrentState != States.IDLE)
        {
            m_SpaceshipBehaviour.UsePrimaryInventorySlot();
            m_CurrentTime = 0.0f;
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
