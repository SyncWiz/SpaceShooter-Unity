using UnityEngine;

public class ShooterEnemyController : MonoBehaviour
{
    //Public
    public float m_CenterDestinationPositionOffset;
    public float m_DistanceAccelerating;
    public float m_DistanceToChangeDestination;
    public float m_CameraLimitYOffset;
    public bool m_InvertHalf;
    public bool m_IsStatic;

    //Private
    private EnemyStates m_CurrentState;
    private SpaceshipBehaviour m_SpaceshipBehaviour;
    private Camera m_MainCamera;
    private Vector3 m_TargetPosition;

    void Start()
    {
        m_CurrentState = EnemyStates.IDLE;
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_MainCamera = Camera.main;
        GenerateTargetPosition();
    }

    void FixedUpdate()
    {
        switch(m_CurrentState)
        {
            case EnemyStates.IDLE:
                if (MathUtils.IsPointInsideCameraView(transform.position))
                {
                    if (!m_IsStatic)
                    {
                        m_CurrentState = EnemyStates.IMPULSE;
                    }
                    else
                    {
                        m_CurrentState = EnemyStates.WAIT_SHOOT;
                    }
                }
                break;
            case EnemyStates.IMPULSE:
                Vector2 force = new Vector2(0, m_TargetPosition.y);
                m_SpaceshipBehaviour.MoveForce(force);
                if (Vector3.Distance(m_TargetPosition, transform.position) <= m_DistanceAccelerating)
                {
                    GenerateTargetPosition();
                }
                m_SpaceshipBehaviour.UsePrimaryInventorySlot(FireType.DEFAULT);
                break;
            case EnemyStates.WAIT:
                if (Vector3.Distance(m_TargetPosition, transform.position) <= m_DistanceToChangeDestination)
                {
                    m_CurrentState = EnemyStates.IMPULSE;
                }
                m_SpaceshipBehaviour.UsePrimaryInventorySlot(FireType.DEFAULT);
                break;
            case EnemyStates.WAIT_SHOOT:
                m_SpaceshipBehaviour.UsePrimaryInventorySlot(FireType.DEFAULT);
            break;
        }

        if (Debug.isDebugBuild)
        {
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
}
