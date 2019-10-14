using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpaceshipBehaviour m_MainSpaceshipBehaviour;

    private void Start()
    {
        m_MainSpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        m_MainSpaceshipBehaviour.Move(moveHorizontal, moveVertical);
        
        if (Input.GetButton("Fire1"))
        {
            m_MainSpaceshipBehaviour.UsePrimaryInventorySlot();
        }

        if (Input.GetButton("Fire2"))
        {
            m_MainSpaceshipBehaviour.UseSecondaryInventorySlot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //TODO update this in the future
        if (collider.gameObject.tag == "EnemyBullet")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<BulletBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "Enemy")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(1);
        }
    }
}
