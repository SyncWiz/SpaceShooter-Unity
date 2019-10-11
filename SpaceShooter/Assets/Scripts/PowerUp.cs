using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public ItemType m_ItemType;
    public float m_MovementSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainShip")
        {
            switch(m_ItemType)
            {
                case ItemType.DoubleFire:
                    collision.gameObject.GetComponent<Inventory>().SetPrimarySlot(m_ItemType);
                break;
                case ItemType.Missile:
                case ItemType.Invulnerability:
                    Debug.Log("Item type adquired: "+m_ItemType);
                    collision.gameObject.GetComponent<Inventory>().SetSecondarySlot(m_ItemType);
                break;
            }
            Destroy(gameObject);
            //TODO sound
        }
    }
}
