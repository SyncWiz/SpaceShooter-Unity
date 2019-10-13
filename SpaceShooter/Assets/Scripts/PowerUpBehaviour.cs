using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    //public
    public ItemType m_ItemType;
    public float m_MovementSpeed;

    //Private
    Vector3 m_Direction;
    Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_Direction = Vector3.up;
    }

    private void Update()
    {
        CheckDirection();
        Move();
    }

    void CheckDirection()
    {
        Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
        if (viewPosition.y > 1.0f)
        {
            m_Direction = Vector3.down;
        }
        else if (viewPosition.y < 0.0f)
        {
            m_Direction = Vector3.up;
        }
    }

    private void Move()
    {
        transform.position += m_Direction * m_MovementSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainShip")
        {
            switch(m_ItemType)
            {
                case ItemType.DoubleFire:
                    collision.gameObject.GetComponent<InventoryManager>().SetPrimarySlot(m_ItemType);
                break;
                case ItemType.Missile:
                case ItemType.Invulnerability:
                    collision.gameObject.GetComponent<InventoryManager>().SetSecondarySlot(m_ItemType);
                break;
            }
            Destroy(gameObject);
            //TODO sound
        }
    }
}
