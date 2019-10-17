using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
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
            case "ResumeGame":
                GameFlowManager.Instance.ResumeGame();
            break;

            case "ExitGame":
                GameFlowManager.Instance.ReturnToMainMenu();
            break;

            default:
                Debug.LogError("Button name must match option name. Check PauseMenuController script.");
            break;
        }
    }
}
