using UnityEngine;
using UnityEngine.UI;

public class DeadMenuController : MonoBehaviour
{
    //Private
    private Button[] m_Buttons;
    private Text[] m_Texts;
    private Text m_ScoreText;

    private void Start()
    {
        m_Buttons = GetComponentsInChildren<Button>();
        foreach (Button button in m_Buttons)
        {
            button.onClick.AddListener(() => RunOption(button.name));
        }

        m_Texts = GetComponentsInChildren<Text>();
        foreach (Text text in m_Texts)
        {
            if(text.name == "Score")
            {
                m_ScoreText = text;
                m_ScoreText.text = "Score: " + GameFlowManager.Instance.GetScore();
                break;
            }
        }
        GameFlowManager.Instance.m_ScoreChangedEvent.AddListener(UpdateScore);
    }

    private void UpdateScore()
    {
        m_ScoreText.text = "Score: " + GameFlowManager.Instance.GetScore();
    }


    public void RunOption(string option)
    {
        switch (option)
        {
            case "PlayAgain":
                GameFlowManager.Instance.StartGame();
                break;

            case "Return":
                GameFlowManager.Instance.ReturnToMainMenuFromDeadMenu();
            break;

            default:
                Debug.LogError("Button name must match option name. Check DeadMenuController script.");
            break;
        }
    }
}
