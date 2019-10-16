using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private enum BossStates
    {
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        INVULNERAVILITY
    }

    //Public 
    public float m_TimeBetweenActions;
    public float m_TopLimit;
    public float m_BottomLimit;
    public float m_VerticalSpeed;

    //Private
    BossStates m_CurrentState;
    SpaceshipBehaviour m_SpaceshipBehaviour;
    InventoryBehaviour m_InventoryBehaviour;
    private bool dirTop = true;
    private float m_CurrentTime;


    void Start()
    {
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_InventoryBehaviour = GetComponent<InventoryBehaviour>();
    }

    void Update()
    {
        Move();
        UpdateTimer();
        UpdateState();
    }

    void Move()
    {
        transform.position += new Vector3(1, 0, 0) * 1 * Time.deltaTime;

        if (dirTop)
            transform.Translate(new Vector3(1, 0, 0) * m_VerticalSpeed * Time.deltaTime);
        else
            transform.Translate(new Vector3(-1, 0, 0) * m_VerticalSpeed * Time.deltaTime);

        if (transform.position.y >= m_TopLimit)
        {
            dirTop = false;
        }

        if (transform.position.y <= m_BottomLimit)
        {
            dirTop = true;
        }
    }

    void UpdateTimer()
    {
        m_CurrentTime += Time.deltaTime;
    }

    void UpdateState()
    {
        if(m_CurrentTime >= m_TimeBetweenActions)
        {
            BossStates action = (BossStates) Random.Range(0, 4);
            switch (action)
            {
                case BossStates.ATTACK_1:
                    m_SpaceshipBehaviour.UsePrimaryInventorySlot(FireType.SINUSOIDAL);
                break;
                case BossStates.ATTACK_2:
                    m_SpaceshipBehaviour.UsePrimaryInventorySlot(FireType.CIRCLE);
                break;
                case BossStates.ATTACK_3:
                    m_InventoryBehaviour.SetSecondarySlot(ItemType.Missile);
                    m_SpaceshipBehaviour.UseSecondaryInventorySlot();
                break;
                case BossStates.INVULNERAVILITY:
                    m_SpaceshipBehaviour.ApplyInvulnerabilityPowerup();
                break;
            }
            m_CurrentTime = 0;
        }

    }
}
