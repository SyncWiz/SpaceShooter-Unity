using System;
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
    public float m_TimeBetweenActionsInRage;
    public float m_TopLimit;
    public float m_BottomLimit;
    public float m_HorizontalSpeed;
    public float m_VerticalSpeed;
    public float m_VerticalSpeedInRage;
    public float m_CameraOffsetXToStart;
    public Color m_ColorRageMode;


    //Private
    private SpaceshipBehaviour m_SpaceshipBehaviour;
    private InventoryBehaviour m_InventoryBehaviour;
    private ReceiveDamageEffect m_ReceiveDamageEffect;
    private bool m_MovingTop;
    private bool m_IsRageMode;
    private bool m_IsInsideCamera;
    private float m_CurrentTime;
    private int m_MaxHealth;
    private SpriteRenderer m_SpriteRenderer;
    private AudioSource m_RageEnterSound;

    void Start()
    {
        m_IsInsideCamera = false;
        m_MovingTop = true;
        m_IsRageMode = false;
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
        m_InventoryBehaviour = GetComponent<InventoryBehaviour>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_ReceiveDamageEffect = GetComponent<ReceiveDamageEffect>();
        m_MaxHealth = m_SpaceshipBehaviour.m_Health;
        m_SpaceshipBehaviour.m_CanRecieveDamage = false;

        AudioSource[] sources = GetComponents<AudioSource>();
        m_RageEnterSound = sources[sources.Length - 1];
    }

    void Update()
    {
        if(!m_IsInsideCamera)
        {
            CheckIsInsideCamera();
        }
        else
        {
            UpdateTimer();
            UpdateState();
        }
    }

    void CheckIsInsideCamera()
    {
        Vector3 position = new Vector3(transform.position.x + m_CameraOffsetXToStart, transform.position.y, transform.position.z);
        m_IsInsideCamera = MathUtils.IsPointInsideCameraView(position);
        if(m_IsInsideCamera)
        {
            m_SpaceshipBehaviour.m_CanRecieveDamage = true;
            m_RageEnterSound.PlayOneShot(m_RageEnterSound.clip);
        }
    }

    void Move()
    {
        if (m_MovingTop)
        {
            m_SpaceshipBehaviour.MoveTranslate(new Vector3(m_VerticalSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            m_SpaceshipBehaviour.MoveTranslate(new Vector3(-m_VerticalSpeed * Time.deltaTime, 0, 0));

        }
        m_SpaceshipBehaviour.MoveTranslate(new Vector3(0, -m_HorizontalSpeed * Time.deltaTime, 0));

        if (transform.position.y >= m_TopLimit)
        {
            m_MovingTop = false;
        }

        if (transform.position.y <= m_BottomLimit)
        {
            m_MovingTop = true;
        }
    }

    void UpdateTimer()
    {
        m_CurrentTime += Time.deltaTime;
    }

    void UpdateState()
    {
        Move();
        if (m_CurrentTime >= m_TimeBetweenActions)
        {
            BossStates action = (BossStates) UnityEngine.Random.Range(0, Enum.GetNames(typeof(BossStates)).Length);
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
                    m_InventoryBehaviour.SetSecondarySlot(ItemType.Invulnerability);
                    m_SpaceshipBehaviour.UseSecondaryInventorySlot();
                break;
            }
            m_CurrentTime = 0;
        }

        if(!m_IsRageMode && m_SpaceshipBehaviour.m_Health <= m_MaxHealth / 2)
        {
            EnterRageMode();
        }
    }

    void EnterRageMode()
    {
        m_RageEnterSound.PlayOneShot(m_RageEnterSound.clip);
        m_IsRageMode = true;
        m_ReceiveDamageEffect.m_OriginalColor = m_ColorRageMode;
        m_SpaceshipBehaviour.m_OriginalColor = m_ColorRageMode;
        m_SpriteRenderer.color = m_ColorRageMode;
        m_TimeBetweenActions = m_TimeBetweenActionsInRage;
        m_VerticalSpeed = m_VerticalSpeedInRage;
    }
}
