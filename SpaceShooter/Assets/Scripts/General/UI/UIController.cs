using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController g_UIController;
    public static UIController Instance
    {
        get
        {
            if (!g_UIController)
            {
                g_UIController = FindObjectOfType(typeof(UIController)) as UIController;

                if (!g_UIController)
                {
                    Debug.LogError("There needs to be one active UIController script on a GameObject in your scene.");
                }
            }

            return g_UIController;
        }
    }

    //Private
    private Dictionary<string, GameObject> m_Menus;

    void Awake()
    {
        if (g_UIController == null)
        {
            DontDestroyOnLoad(this.gameObject);
            g_UIController = this;
            m_Menus = new Dictionary<string, GameObject>();
            m_Menus.Add("MainMenu", FindObjectOfType<MainMenuController>().gameObject);
            m_Menus.Add("PauseMenu", FindObjectOfType<PauseMenuController>().gameObject);
            m_Menus.Add("DeadMenu", FindObjectOfType<DeadMenuController>().gameObject);
            m_Menus.Add("WinMenu", FindObjectOfType<WinMenuController>().gameObject);
            HideAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show(string menuName)
    {
        m_Menus[menuName].SetActive(true);
    }

    public void Hide(string menuName)
    {
        m_Menus[menuName].SetActive(false);
    }


    public void HideAll()
    {
        foreach (GameObject menu in m_Menus.Values)
            menu.SetActive(false);
    }
}
