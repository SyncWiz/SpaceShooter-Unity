using System.Collections.Generic;
using UnityEngine;

public class MainGameUIController : MonoBehaviour
{
    //Private
    private Dictionary<string, GameObject> m_Menus;

    void Awake()
    {
        m_Menus = new Dictionary<string, GameObject>();
        m_Menus.Add("HUD", FindObjectOfType<HUDController>().gameObject);
    }

    public void HideAll()
    {
        foreach (GameObject menu in m_Menus.Values)
        {
            menu.SetActive(false);
        }
    }

    public void ShowAll()
    {
        foreach (GameObject menu in m_Menus.Values)
        {
            menu.SetActive(true);
        }
    }
}
