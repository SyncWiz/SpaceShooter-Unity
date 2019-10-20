using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    //Public
    public Vector3 m_Direction;
    public float m_Speed;
    public int m_Damage;
    public bool m_AllyBullet;
    public bool m_SinusoidalMovement;

    //Private
    private string m_TagDetection;
    private Vector3 m_StartPosition;

    void Start()
    {
        m_StartPosition = transform.position;
        if (m_AllyBullet)
        {
            m_TagDetection = "Enemy";
        }
        else
        {
            m_TagDetection = "MainShip";
        }
    }

    void Update()
    {
        Move();
        if(!MathUtils.IsPointInsideCameraView(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        if(!m_SinusoidalMovement)
        {
            transform.position += m_Direction * m_Speed * Time.deltaTime;
        }
        else
        {
            transform.position = new Vector3(transform.position.x + (m_Direction.x * m_Speed * Time.deltaTime), m_StartPosition.y + Mathf.Sin(Time.time * m_Speed), 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == m_TagDetection)
        {
            if(m_AllyBullet)
            {
                Destroy(gameObject);
            }
            else if(collider.gameObject.GetComponent<SpaceshipBehaviour>().m_CanRecieveDamage)
            {
                Destroy(gameObject);
            }
        }

        if(collider.gameObject.tag == "Asteroid")
        {
            if (m_AllyBullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
