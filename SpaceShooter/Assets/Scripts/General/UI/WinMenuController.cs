using UnityEngine;
using UnityEngine.UI;

public class WinMenuController : MonoBehaviour
{
    //Public
    public GameObject m_ButtonSound;

    //Private
    private Text m_ScoreText;
    private AudioSource m_AudioSource;

    private void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => RunOption(button.name));
        }

        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name == "Score")
            {
                m_ScoreText = text;
                UpdateScore();
                break;
            }
        }
        m_AudioSource = m_ButtonSound.GetComponent<AudioSource>();
        GameFlowManager.Instance.m_ScoreChangedEvent.AddListener(UpdateScore);
    }

    private void UpdateScore()
    {
        m_ScoreText.text = "Score: " + GameFlowManager.Instance.GetScore().ToString();
    }


    public void RunOption(string option)
    {
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        switch (option)
        {
            case "PlayAgain":
                GameFlowManager.Instance.StartGame();
                break;

            case "Return":
                GameFlowManager.Instance.ShowMainMenu();
                break;

            default:
                Debug.LogError("Button name must match option name. Check DeadMenuController script.");
                break;
        }
    }
}
