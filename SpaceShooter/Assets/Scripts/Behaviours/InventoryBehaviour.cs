using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Empty,
    BasicFire,
    DoubleFire,
    Missile,
    Invulnerability
}

public class InventoryBehaviour : MonoBehaviour
{
    private struct InventorySlot
    {
        private ItemType m_CurrentItemType;
        public void SetCurrentItem(ItemType item)
        {
            m_CurrentItemType = item;
        }

        public ItemType GetCurrentItem()
        {
            return m_CurrentItemType;
        }
    }

    //Public
    public float m_DoubleFireTime;
    public bool m_IsMainInventory;
    public UnityEvent m_PrimarySlotChangedEvent;
    public UnityEvent m_SecondarySlotChangedEvent;

    //Private
    private InventorySlot m_PrimarySlot, m_SecondarySlot;

    void Start()
    {
        SetPrimarySlot(ItemType.BasicFire);
        SetSecondarySlot(ItemType.Empty);
    }

    public void SetPrimarySlot(ItemType item)
    {
        Debug.Assert(item == ItemType.BasicFire || item == ItemType.DoubleFire);
        m_PrimarySlot.SetCurrentItem(item);
        if(m_IsMainInventory)
        {
            m_PrimarySlotChangedEvent.Invoke();
        }
        if (item == ItemType.DoubleFire)
        {
            Invoke("SetBasicFire", m_DoubleFireTime);
        }
    }

    public void SetSecondarySlot(ItemType item)
    {
        Debug.Assert(item == ItemType.Missile || item == ItemType.Invulnerability || item == ItemType.Empty);
        m_SecondarySlot.SetCurrentItem(item);
        if (m_IsMainInventory)
        {
            m_SecondarySlotChangedEvent.Invoke();
        }
    }

    public ItemType GetPrimarySlot()
    {
        return m_PrimarySlot.GetCurrentItem();
    }

    public ItemType GetSecondarySlot()
    {
        return m_SecondarySlot.GetCurrentItem();
    }

    private void SetBasicFire()
    {
        m_PrimarySlot.SetCurrentItem(ItemType.BasicFire);
        if (m_IsMainInventory)
        {
            m_PrimarySlotChangedEvent.Invoke();
        }
    }
}
