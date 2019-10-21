using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    //public
    public ItemType m_ItemType;
    public float m_MovementSpeed;

    //Private
    Vector3 m_Direction;
    Camera m_MainCamera;
    AudioSource m_PowerupSound;

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_Direction = Vector3.up;
        m_PowerupSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckDirection();
        Move();
    }

    void CheckDirection()
    {
        Vector3 viewPosition = m_MainCamera.WorldToViewportPoint(transform.position);
        if (viewPosition.y > 1.0f)
        {
            m_Direction = Vector3.down;
        }
        else if (viewPosition.y < 0.0f)
        {
            m_Direction = Vector3.up;
        }
    }

    private void Move()
    {
        transform.position += m_Direction * m_MovementSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainShip")
        {
            m_PowerupSound.PlayOneShot(m_PowerupSound.clip);
            switch (m_ItemType)
            {
                case ItemType.DoubleFire:
                    collision.gameObject.GetComponent<InventoryBehaviour>().SetPrimarySlot(m_ItemType);
                break;
                case ItemType.Missile:
                case ItemType.Invulnerability:
                    collision.gameObject.GetComponent<InventoryBehaviour>().SetSecondarySlot(m_ItemType);
                break;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 1.0f);
        }
    }
}
