using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    CIRCLE
}

public class SpaceshipBehaviour : MonoBehaviour
{
    //Public
    public float m_Speed;
    public float m_BulletOffsetX;
    public float m_TimeBetweenPrimaryShoots;
    public float m_InvulnerabilityTime;
    public float m_DamageReceivedTime;
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
    public Color m_InvulnerabilityColor;
    public Color m_DamageReceivedColor;

    //Private
    private float m_CurrentTime;
    private InventoryBehaviour m_Inventory;
    private SpriteRenderer m_SpriteRenderer;
    private Color m_OriginalColor;
    private Rigidbody2D m_RigidBody2D;
    private BulletBehaviour m_BulletBehaviour;

    #region General behaviour  
    void Start()
    {
        m_Inventory = GetComponent<InventoryBehaviour>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if(m_CanShoot)
        {
            m_BulletBehaviour = m_BasicBullet.GetComponent<BulletBehaviour>();
        }
        m_OriginalColor = m_SpriteRenderer.color;
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_CurrentTime = 0.0f;
        m_CanRecieveDamage = true;
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        m_CurrentTime += Time.deltaTime;
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
                        Fire(bulletPosition, m_BasicBullet, m_BasicBulletRotation);
                    }
                    else if(fireType == FireType.CIRCLE)
                    {
                        FireInCircle(m_BasicBullet);
                    }
                break;
                case ItemType.DoubleFire:
                    Vector3 bulletPosition1 = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y + 0.15f, transform.position.z);
                    Vector3 bulletPosition2 = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y - 0.15f, transform.position.z);
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
                Instantiate(m_MissileBullet, missilePosition, Quaternion.identity);
                //TODO sound
                break;
            case ItemType.Invulnerability:
                m_CanRecieveDamage = false;
                m_InvulnerabilityCircle.SetActive(true);
                StartCoroutine(InvulnerabilityEffect(m_InvulnerabilityColor));
                Invoke("ReturnFromInvulnerabilityState", m_InvulnerabilityTime);
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
            Vector3 position = GetPositionAtCircle(center, m_RadiusCircleBullets, currentAngle);
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

        if (m_IsMainPlayer)
        {
            m_CanRecieveDamage = false;
            StartCoroutine(InvulnerabilityEffect(m_DamageReceivedColor));
            Invoke("ReturnFromInvulnerabilityState", m_DamageReceivedTime);
            //TODO sound
        }
        else
        {
            m_SpriteRenderer.color = m_DamageReceivedColor;
            Invoke("RecoverOriginalColor", m_DamageReceivedTime);
            //TODO sound
        }

        if (m_Health <= 0)
        {
            Invoke("Die", m_TimeUntilDie);
            //TODO gameflow controller force endgame
        }

    }

    public void Die()
    {
        Destroy(gameObject);
        //TODO sound + effect
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
                RecoverOriginalColor();
            }
            yield return new WaitForSeconds(m_TimeBetweenChangingColor);
        }
    }
    #endregion

    #region Utils
    void RecoverOriginalColor()
    {
        m_SpriteRenderer.color = m_OriginalColor;
    }

    Vector3 GetPositionAtCircle(Vector3 center, float radius, float degreeAngle)
    {
        Vector3 position;
        position.x = center.x + (radius * Mathf.Sin(degreeAngle * Mathf.Deg2Rad));
        position.y = center.y + (radius * Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
        position.z = center.z;
        return position;
    }
    #endregion
}
