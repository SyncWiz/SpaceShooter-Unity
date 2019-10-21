using UnityEngine;

public class HelixEnemyController : MonoBehaviour
{
    //Private
    private SpaceshipBehaviour m_SpaceshipBehaviour;
    private EnemyStates m_CurrentState;

    void Start()
    {
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_CurrentState = EnemyStates.IDLE;
    }

    void Update()
    {
        switch (m_CurrentState)
        {
            case EnemyStates.IDLE:
                if (MathUtils.IsPointInsideCameraView(transform.position))
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
