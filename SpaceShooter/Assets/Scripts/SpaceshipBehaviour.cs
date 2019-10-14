using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool m_IsMainPlayer;
    [HideInInspector]
    public bool m_CanRecieveDamage;
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
    private InventoryManager m_Inventory;
    private SpriteRenderer m_SpriteRenderer;
    private Color m_OriginalColor;
    private Rigidbody2D m_RigidBody2D;


    void Start()
    {
        m_Inventory = GetComponent<InventoryManager>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalColor = m_SpriteRenderer.color;
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_CurrentTime = 0.0f;
        m_CanRecieveDamage = true;
    }

    void Update()
    {
        UpdateTimers();
    }

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

    void UpdateTimers()
    {
        m_CurrentTime += Time.deltaTime;
    }

    public void UsePrimaryInventorySlot()
    {
        if (m_CurrentTime >= m_TimeBetweenPrimaryShoots)
        {
            ItemType item = m_Inventory.GetPrimarySlot();
            switch (item)
            {
                case ItemType.BasicFire:
                    Vector3 bulletPosition = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y, transform.position.z);
                    Instantiate(m_BasicBullet, bulletPosition, Quaternion.Euler(m_BasicBulletRotation));
                    break;
                case ItemType.DoubleFire:
                    Vector3 bulletPosition1 = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y + 0.15f, transform.position.z);
                    Vector3 bulletPosition2 = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y - 0.15f, transform.position.z);
                    Instantiate(m_DoubleBullet, bulletPosition1, Quaternion.identity);
                    Instantiate(m_DoubleBullet, bulletPosition2, Quaternion.identity);
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

    void RecoverOriginalColor()
    {
        m_SpriteRenderer.color = m_OriginalColor;
    }

    IEnumerator InvulnerabilityEffect(Color color)
    {
        while (!m_CanRecieveDamage)
        {
            if (m_SpriteRenderer.color != color)
            {
                m_SpriteRenderer.color = color;
            }
            else if(m_SpriteRenderer.color != m_OriginalColor)
            {
                RecoverOriginalColor();
            }
            yield return new WaitForSeconds(m_TimeBetweenChangingColor);
        }
    }
}
