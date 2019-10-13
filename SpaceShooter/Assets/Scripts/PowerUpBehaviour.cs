using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    public ItemType m_ItemType;
    public float m_MovementSpeed;

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
