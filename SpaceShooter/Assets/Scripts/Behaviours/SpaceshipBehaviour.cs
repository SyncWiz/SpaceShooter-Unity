using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyStates
{
    IDLE,
    MOVE,
    MOVE_TO_POINT,
    IMPULSE,
    WAIT,
    WAIT_SHOOT
}

public enum FireType
{
    DEFAULT,
    CIRCLE,
    SINUSOIDAL
}

public class SpaceshipBehaviour : MonoBehaviour
{
    //Public
    public int m_Points;
    public float m_Speed;
    public float m_BulletOffsetX;
    public float m_DoubleBulletOffsetY;
    public float m_TimeBetweenPrimaryShoots;
    public float m_InvulnerabilityTime;
    public float m_TimeBetweenChangingColor;
    public float m_TimeUntilDie;
    public float m_NumberOfBulletsInCircle;
    public float m_RadiusCircleBullets;
    public bool m_IsMainPlayer;
    [HideInInspector]
    public bool m_CanRecieveDamage;
    public bool m_CanShoot;
    public int m_Health;
    public GameObject m_BasicBullet;
    public GameObject m_DoubleBullet;
    public GameObject m_MissileBullet;
    public GameObject m_InvulnerabilityCircle;
    public Vector3 m_BasicBulletRotation;
    public Vector3 m_MissileBulletRotation;
    public Color m_InvulnerabilityColor;
    [HideInInspector]
    public Color m_OriginalColor;
    public UnityEvent m_HealthChangedEvent;

    //Private
    private float m_CurrentTime;
    private InventoryBehaviour m_Inventory;
    private SpriteRenderer m_SpriteRenderer;
    
    private Rigidbody2D m_RigidBody2D;
    private BulletBehaviour m_BulletBehaviour;
    private ReceiveDamageEffect m_ReceiveDamageEffect;
    private Vector3 m_OriginalBulletDirection;

    void Awake()
    {
        m_CanRecieveDamage = true;
    }

    #region General behaviour  
    void Start()
    {
        m_Inventory = GetComponent<InventoryBehaviour>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_ReceiveDamageEffect = GetComponent<ReceiveDamageEffect>();
        if (m_CanShoot)
        {
            m_BulletBehaviour = m_BasicBullet.GetComponent<BulletBehaviour>();
            m_OriginalBulletDirection = m_BulletBehaviour.m_Direction;
        }
        m_OriginalColor = m_SpriteRenderer.color;
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_CurrentTime = 0.0f;
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        m_CurrentTime += Time.deltaTime;
    }

    void OnApplicationQuit()
    {
        if (m_CanShoot)
        {
            m_BulletBehaviour.m_Direction = m_OriginalBulletDirection;
        }
    }
    #endregion

    #region Movement
    public void Move(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * m_Speed * Time.deltaTime;
    }

    public void MoveForce(Vector2 force)
    {
        m_RigidBody2D.AddForce(force);
    }

    public void MoveTo(Vector3 destination)
    {
        transform.position = Vector3.Lerp(transform.position, destination, m_Speed * Time.deltaTime);
    }

    public void MoveTranslate(Vector3 point)
    {
        transform.Translate(point);
    }
    #endregion

    #region Inventory
    public void UsePrimaryInventorySlot(FireType fireType)
    {
        if (m_CurrentTime >= m_TimeBetweenPrimaryShoots)
        {
            ItemType item = m_Inventory.GetPrimarySlot();
            switch (item)
            {
                case ItemType.BasicFire:
                    if(fireType == FireType.DEFAULT)
                    {
                        Vector3 bulletPosition = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y, transform.position.z);
                        m_BulletBehaviour.m_SinusoidalMovement = false;
                        m_BulletBehaviour.m_Direction = m_OriginalBulletDirection;
                        Fire(bulletPosition, m_BasicBullet, m_BasicBulletRotation);
                    }
                    else if(fireType == FireType.SINUSOIDAL)
                    {
                        Vector3 bulletPosition = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y, transform.position.z);
                        m_BulletBehaviour.m_Direction = m_OriginalBulletDirection;
                        m_BulletBehaviour.m_SinusoidalMovement = true;
                        Fire(bulletPosition, m_BasicBullet, m_BasicBulletRotation);
                    }
                    else if(fireType == FireType.CIRCLE)
                    {
                        m_BulletBehaviour.m_SinusoidalMovement = false;
                        FireInCircle(m_BasicBullet);
                    }
                break;
                case ItemType.DoubleFire:
                    Vector3 bulletPosition1 = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y + m_DoubleBulletOffsetY, transform.position.z);
                    Vector3 bulletPosition2 = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y - m_DoubleBulletOffsetY, transform.position.z);
                    Fire(bulletPosition1, m_DoubleBullet, Vector3.zero);
                    Fire(bulletPosition2, m_DoubleBullet, Vector3.zero);
                break;
            }
            m_CurrentTime = 0.0f;
        }
    }

    public void UseSecondaryInventorySlot()
    {
        ItemType item = m_Inventory.GetSecondarySlot();
        switch (item)
        {
            case ItemType.Missile:
                Vector3 missilePosition = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y, transform.position.z);
                Fire(missilePosition, m_MissileBullet, m_MissileBulletRotation);
                //TODO sound
            break;
            case ItemType.Invulnerability:
                ApplyInvulnerabilityPowerup();
                //TODO sound
            break;
            case ItemType.Empty:
                //TODO sound or somefeedback
            break;
        }
        m_Inventory.SetSecondarySlot(ItemType.Empty);
    }
    #endregion

    #region Fire
    void Fire(Vector3 position, GameObject bullet, Vector3 rotation)
    {
        Instantiate(bullet, position, Quaternion.Euler(rotation));
    }

    void FireInCircle(GameObject bullet)
    {
        Vector3 center = transform.position;
        float angleStep = 360 / m_NumberOfBulletsInCircle;
        float currentAngle = 0;
        for (int i = 0; i < m_NumberOfBulletsInCircle; i++)
        {
            Vector3 position = MathUtils.GetPositionAtCircle(center, m_RadiusCircleBullets, currentAngle);
            currentAngle += angleStep;
            Vector3 direction = position - transform.position;
            direction.z = 0;
            direction.Normalize();
            m_BulletBehaviour.m_Direction = direction;
            Instantiate(bullet, position, Quaternion.identity);
        }
    }
    #endregion

    #region Damage
    public void ReceiveDamage(int damage)
    {
        if (!m_CanRecieveDamage)
            return;

        m_Health -= damage;

        if(m_IsMainPlayer)
        {
            m_CanRecieveDamage = false;
            m_HealthChangedEvent.Invoke();
            StartCoroutine(InvulnerabilityEffect(m_ReceiveDamageEffect.m_DamageReceivedColor));
            Invoke("ReturnFromInvulnerabilityState", m_ReceiveDamageEffect.m_DamageReceivedTime);
        }
        else
        {
            m_ReceiveDamageEffect.ApplyEffect();
        }

        if (m_Health <= 0)
        {
            Invoke("Die", m_TimeUntilDie);
            //TODO gameflow controller force endgame
        }
    }

    void Die()
    {
        if(m_IsMainPlayer)
        {
            GameFlowManager.Instance.EndGame();
        }
        else
        {
            GameFlowManager.Instance.AddScorePoints(m_Points);
        }
        Destroy(gameObject);
        //TODO sound + effect
    }

    public void ApplyInvulnerabilityPowerup()
    {
        m_CanRecieveDamage = false;
        StopAllCoroutines();
        CancelInvoke("ReturnFromInvulnerabilityState");
        m_SpriteRenderer.color = m_OriginalColor;
        m_InvulnerabilityCircle.SetActive(true);
        StartCoroutine(InvulnerabilityEffect(m_InvulnerabilityColor));
        Invoke("ReturnFromInvulnerabilityState", m_InvulnerabilityTime);
    }

    void ReturnFromInvulnerabilityState()
    {
        m_CanRecieveDamage = true;
        m_SpriteRenderer.color = m_OriginalColor;
        m_InvulnerabilityCircle.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator InvulnerabilityEffect(Color color)
    {
        while (!m_CanRecieveDamage)
        {
            if (m_SpriteRenderer.color != color)
            {
                m_SpriteRenderer.color = color;
            }
            else if (m_SpriteRenderer.color != m_OriginalColor)
            {
                m_SpriteRenderer.color = m_OriginalColor;
            }
            yield return new WaitForSeconds(m_TimeBetweenChangingColor);
        }
    }
    #endregion
}
