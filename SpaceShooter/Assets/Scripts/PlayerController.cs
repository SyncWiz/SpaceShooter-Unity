using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public
    public float m_Speed;
    public float m_BasicBulletOffset;
    public float m_TimeBetweenBasicShoots;
    public GameObject m_BasicBullet;

    //Private
    private Rigidbody2D m_Rigidbody2d;
    private float m_CurrenTime = 0.0f;


    void Start()
    {
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
        m_Rigidbody2d.gravityScale = 0.0f;
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
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        m_Rigidbody2d.AddForce(movement * m_Speed);
    }

    void UpdateTimers()
    {
        m_CurrenTime += Time.deltaTime;
    }

    void ProcessInput()
    {
        if (Input.GetButton("Fire1"))
        {
            ShootBasicProjectile();
            return;
        }
        if(Input.GetButton("Fire2"))
        {
            ShootPoweredProjectile();
            return;
        }
    }

    void ShootBasicProjectile()
    {
        if(m_CurrenTime >= m_TimeBetweenBasicShoots)
        {
            Vector3 bulletPosition = new Vector3(transform.position.x + m_BasicBulletOffset, transform.position.y, transform.position.z);
            Instantiate(m_BasicBullet, transform.position, Quaternion.identity);
            m_CurrenTime = 0.0f;
        }
    }

    void ShootPoweredProjectile()
    {
        //TODO
    }
}
