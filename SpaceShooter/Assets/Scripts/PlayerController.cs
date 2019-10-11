using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public
    public float m_Speed;
    public float m_BulletOffsetX;
    public float m_TimeBetweenPrimaryShoots;
    public GameObject m_BasicBullet;
    public GameObject m_DoubleBullet;
    public GameObject m_MissileBullet;
    public int m_Health;
    public float m_InvulneravilityTime;
    public Color m_InvulneravilityColor;

    //Private
    private float m_CurrentTime;
    private Inventory m_Inventory;
    private bool m_CanRecieveDamage;
    private SpriteRenderer m_SpriteRenderer;
    private Color m_OriginalColor;


    void Start()
    {
        m_Inventory = GetComponent<Inventory>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_OriginalColor = m_SpriteRenderer.color;
        m_Health = 3;
        m_CurrentTime = 0.0f;
        m_CanRecieveDamage = true;
    }

    void Update()
    {
        Move();
        UpdateTimers();
        ProcessInput();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * m_Speed * Time.deltaTime;
    }

    void UpdateTimers()
    {
        m_CurrentTime += Time.deltaTime;
    }

    void ProcessInput()
    {
        if (Input.GetButton("Fire1"))
        {
            UsePrimaryIventorySlot();
        }
        else if(Input.GetButton("Fire2") || Input.GetKeyDown(KeyCode.LeftControl))
        {
            UseSecondaryIventorySlot();
        }
    }

    void UsePrimaryIventorySlot()
    {
        if(m_CurrentTime >= m_TimeBetweenPrimaryShoots)
        {
            ItemType item = m_Inventory.GetPrimarySlot();
            switch(item)
            {
                case ItemType.BasicFire:
                    Vector3 bulletPosition = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y, transform.position.z);
                    Instantiate(m_BasicBullet, bulletPosition, Quaternion.identity);
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

    void UseSecondaryIventorySlot()
    {
        ItemType item = m_Inventory.GetSecondarySlot();
        Debug.Log(item);
        switch (item)
        {
            case ItemType.Missile:
                Vector3 missilePosition = new Vector3(transform.position.x + m_BulletOffsetX, transform.position.y, transform.position.z);
                Instantiate(m_MissileBullet, missilePosition, Quaternion.identity);
                //TODO sound
            break;
            case ItemType.Invulnerability:
                m_CanRecieveDamage = false;
                StartCoroutine("InvulneravilityEffect");
                Invoke("ReturnFromInvulneravilityState", m_InvulneravilityTime);
                //TODO sound
            break;
            case ItemType.Empty:
            //TODO sound or somefeedback
            break;
        }
        m_Inventory.SetSecondarySlot(ItemType.Empty);
    }

    void ReceiveDamage()
    {
        if (!m_CanRecieveDamage)
            return;

        m_Health--;
        if(m_Health == 0)
        {
            //TODO gameflow controller force endgame
        }
    }

    void ReturnFromInvulneravilityState()
    {
        m_CanRecieveDamage = true;
        m_SpriteRenderer.color = m_OriginalColor;
    }

    IEnumerator InvulneravilityEffect()
    {
        while(!m_CanRecieveDamage)
        {
            if(m_SpriteRenderer.color != m_InvulneravilityColor)
            {
                m_SpriteRenderer.color = m_InvulneravilityColor;
            }
            else
            {
                m_SpriteRenderer.color = m_OriginalColor;
            }
            //TODO expose this value?
            yield return new WaitForSeconds(.25f);
        }
    }
}
