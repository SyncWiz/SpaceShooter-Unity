using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //Public
    public GameObject m_MainPlayer;
    public Color m_HidedObjectColor;

    //Private
    private Text m_ScoreText;
    private Text m_HealthText;
    private RawImage m_BasicShoot;
    private RawImage m_DoubleShoot;
    private RawImage m_MissileShoot;
    private RawImage m_Invulnerability;
    private Color m_OriginalColor;
    private InventoryBehaviour m_InventoryBehaviour;
    private SpaceshipBehaviour m_SpaceshipBehaviour;

    private void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        m_InventoryBehaviour = m_MainPlayer.GetComponent<InventoryBehaviour>();
        m_SpaceshipBehaviour = m_MainPlayer.GetComponent<SpaceshipBehaviour>();

        foreach (Text text in texts)
        {
            switch(text.name)
            {
               case "Score":
                    m_ScoreText = text;
                    UpdateScore();
               break;
               case "Health":
                    m_HealthText = text;
                    UpdateHealth();
                    break;
            }
        }

        RawImage[] rawImages = GetComponentsInChildren<RawImage>();
        foreach (RawImage image in rawImages)
        {
            switch (image.name)
            {
                case "BasicFire":
                    m_BasicShoot = image;
                break;

                case "DoubleFire":
                    m_DoubleShoot = image;
                break;

                case "Missile":
                    m_MissileShoot = image;
                break;

                case "Invulnerability":
                    m_Invulnerability = image;
                break;
            }
        }

        m_OriginalColor = m_BasicShoot.color;
        UpdatePrimarySlot();
        UpdateSecondarySlot();
        GameFlowManager.Instance.m_ScoreChangedEvent.AddListener(UpdateScore);
        m_SpaceshipBehaviour.m_HealthChangedEvent.AddListener(UpdateHealth);
        m_InventoryBehaviour.m_PrimarySlotChangedEvent.AddListener(UpdatePrimarySlot);
        m_InventoryBehaviour.m_SecondarySlotChangedEvent.AddListener(UpdateSecondarySlot);
    }

    private void UpdateScore()
    {
        m_ScoreText.text = GameFlowManager.Instance.GetScore().ToString();
    }

    private void UpdateHealth()
    {
        m_HealthText.text = m_SpaceshipBehaviour.m_Health.ToString(); ;
    }

    private void UpdatePrimarySlot()
    {
        switch(m_InventoryBehaviour.GetPrimarySlot())
        {
            case ItemType.BasicFire:
                m_BasicShoot.color = m_OriginalColor;
                m_DoubleShoot.color = m_HidedObjectColor;
            break;
            case ItemType.DoubleFire:
                m_BasicShoot.color = m_HidedObjectColor;
                m_DoubleShoot.color = m_OriginalColor;
            break;
        }
    }

    private void UpdateSecondarySlot()
    {
        switch (m_InventoryBehaviour.GetSecondarySlot())
        {
            case ItemType.Missile:
                m_MissileShoot.color = m_OriginalColor;
                m_Invulnerability.color = m_HidedObjectColor;
            break;
            case ItemType.Invulnerability:
                m_MissileShoot.color = m_HidedObjectColor;
                m_Invulnerability.color = m_OriginalColor;
                break;
            case ItemType.Empty:
                m_MissileShoot.color = m_HidedObjectColor;
                m_Invulnerability.color = m_HidedObjectColor;
            break;
        }
    }
}
