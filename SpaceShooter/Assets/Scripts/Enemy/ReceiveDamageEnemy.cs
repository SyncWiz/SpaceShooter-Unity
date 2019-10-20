using UnityEngine;

public class ReceiveDamageEnemy : MonoBehaviour
{
    //Public
    public int m_AmountDamageSplashExplosion;

    //Private
    private SpaceshipBehaviour m_SpaceshipBehaviour;

    void Start()
    {
        m_SpaceshipBehaviour = GetComponent<SpaceshipBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainShipBullet")
        {
            m_SpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<BulletBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "MainShipMissile")
        {
            m_SpaceshipBehaviour.ReceiveDamage(collider.gameObject.GetComponent<MissileBehaviour>().m_Damage);
        }
        if (collider.gameObject.tag == "MainShipExplosion")
        {
            m_SpaceshipBehaviour.ReceiveDamage(m_AmountDamageSplashExplosion);
        }
    }
}
