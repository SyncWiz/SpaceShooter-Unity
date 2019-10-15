using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    //Public
    public Vector3 m_Direction;
    public float m_Speed;

    void Update()
    {
        transform.position += m_Direction * m_Speed * Time.deltaTime;
    }
}
