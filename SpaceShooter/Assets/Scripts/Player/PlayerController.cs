using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Private
    SpaceshipBehaviour m_MainSpaceshipBehaviour;

    private void Start()
    {
        m_MainSpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
    }

    void Update()
    {
        if(!GameFlowManager.Instance.IsGamePaused())
        {
            ProcessInput();
        }
    }

    void ProcessInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        m_MainSpaceshipBehaviour.Move(moveHorizontal, moveVertical);
        
        if (Input.GetButton("Fire1"))
        {
            m_MainSpaceshipBehaviour.UsePrimaryInventorySlot(FireType.DEFAULT);
        }

        if (Input.GetButton("Fire2"))
        {
            m_MainSpaceshipBehaviour.UseSecondaryInventorySlot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "EnemyBullet")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<BulletBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "EnemyMissile")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<MissileBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Asteroid")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(1);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "EnemyBullet")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<BulletBehaviour>().m_Damage);
        }
        if(collider.gameObject.tag == "EnemyMissile")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<MissileBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Asteroid")
        {
            m_MainSpaceshipBehaviour.ReceiveDamage(1);
        }
    }
}
