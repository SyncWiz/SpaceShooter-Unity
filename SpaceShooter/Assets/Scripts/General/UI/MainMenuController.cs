using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //Private
    private Button[] m_Buttons;
    private void Start()
    {
        m_Buttons = GetComponentsInChildren<Button>();
        foreach (Button button in m_Buttons)
        {
            button.onClick.AddListener(() => RunOption(button.name));
        }
    }
    public void RunOption(string option)
    {
        switch (option)
        {
            case "StartGame":
                GameFlowManager.Instance.StartGame();
            break;

            case "ExitGame":
                GameFlowManager.Instance.ExtiGame();
            break;


            default:
                Debug.LogError("Button name must match option name. Check MainMenuController script.");
            break;
        }
    }
}
