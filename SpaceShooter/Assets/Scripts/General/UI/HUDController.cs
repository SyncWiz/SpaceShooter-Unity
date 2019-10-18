using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //Public
    public GameObject m_MainPlayer;

    //Private
    private Text[] m_Texts;
    private Text m_ScoreText;
    private Text m_HealthText;
    private Text m_PrimarySlotText;
    private Text m_SecondarySlotText;
    private InventoryBehaviour m_InventoryBehaviour;
    private SpaceshipBehaviour m_SpaceshipBehaviour;

    private void Start()
    {
        m_Texts = GetComponentsInChildren<Text>();
        m_InventoryBehaviour = m_MainPlayer.GetComponent<InventoryBehaviour>();
        m_SpaceshipBehaviour = m_MainPlayer.GetComponent<SpaceshipBehaviour>();

        foreach (Text text in m_Texts)
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
               case "SlotOne":
                    m_PrimarySlotText = text;
                    UpdatePrimarySlot();
               break;
               case "SlotTwo":
                    m_SecondarySlotText = text;
                    UpdateSecondarySlot();
               break;
            }
        }
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
        m_PrimarySlotText.text = m_InventoryBehaviour.GetPrimarySlot().ToString();
    }

    private void UpdateSecondarySlot()
    {
        m_SecondarySlotText.text = m_InventoryBehaviour.GetSecondarySlot().ToString();
    }
}
