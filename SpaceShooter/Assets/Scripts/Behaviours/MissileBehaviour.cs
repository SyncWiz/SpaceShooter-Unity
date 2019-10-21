using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
    //Public
    public float m_Speed;
    public float m_TimeToExplode;
    public float m_NumberOfBulletsAfterExplosion;
    public float m_ExplosionRadius;
    public int m_Damage;
    public GameObject m_Explosion;
    public GameObject m_BasicBullet;
    public Vector3 m_Direction;
    public bool m_IsAlly;

    //Private
    private Rigidbody2D m_RigidBody2D;
    private BulletBehaviour m_BulletBehaviour;
    private AudioSource m_ExplosionSound;

    void Start()
    { 
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        Invoke("Explode", m_TimeToExplode);
        m_ExplosionSound = GetComponent<AudioSource>();
        if (!m_IsAlly)
        {
            m_BulletBehaviour = m_BasicBullet.GetComponent<BulletBehaviour>();
        }
    }

    void FixedUpdate()
    {
        m_RigidBody2D.AddForce(m_Direction * m_Speed);
    }

    void Explode()
    {
        if(m_IsAlly)
        {
            Instantiate(m_Explosion, transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 center = transform.position;
            float angleStep = 360 / m_NumberOfBulletsAfterExplosion;
            float currentAngle = 0;

            if (Random.value > 0.5f)
            {
                m_BulletBehaviour.m_SinusoidalMovement = true;
            }
            else
            {
                m_BulletBehaviour.m_SinusoidalMovement = false;
            }

            for (int i = 0; i < m_NumberOfBulletsAfterExplosion; i++)
            {
                Vector3 position = MathUtils.GetPositionAtCircle(center, 1, currentAngle);
                currentAngle += angleStep;
                Vector3 direction = position - transform.position;
                direction.z = 0;
                direction.Normalize();
                m_BulletBehaviour.m_Direction = direction;
                Instantiate(m_BasicBullet, position, Quaternion.identity);
            } 
        }

        m_ExplosionSound.PlayOneShot(m_ExplosionSound.clip);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        CancelInvoke("Explode");
        enabled = false;
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_IsAlly)
        {
            if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Asteroid")
            {
                Explode();
            }
        }
        else
        {
            if(collider.gameObject.tag == "MainShip")
            {
                Explode();
            }
        }

    }
}
