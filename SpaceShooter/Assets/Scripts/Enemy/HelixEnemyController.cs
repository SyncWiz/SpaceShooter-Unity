using UnityEngine;

public class HelixEnemyController : MonoBehaviour
{
    //Private
    private SpaceshipBehaviour m_SpaceshipBehaviour;
    private EnemyStates m_CurrentState;
    private Camera m_MainCamera;

    void Start()
    {
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_CurrentState = EnemyStates.IDLE;
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        switch (m_CurrentState)
        {
            case EnemyStates.IDLE:
                Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
                if (viewPosition.x <= 1.0f && viewPosition.x >= 0.0f && viewPosition.y <= 1.0f && viewPosition.y >= 0.0f)
                {
                    m_CurrentState = EnemyStates.WAIT_SHOOT;
                }
            break;
            case EnemyStates.WAIT_SHOOT:
                m_SpaceshipBehaviour.UsePrimaryInventorySlot(FireType.CIRCLE);
            break;
        }   
   }
}
