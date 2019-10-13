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
        //TODO setup proper fire2
        if (Input.GetButton("Fire2") || Input.GetKeyDown(KeyCode.LeftControl))
        {
            m_MainSpaceshipBehaviour.UseSecondaryInventorySlot();
        }
    }
}
