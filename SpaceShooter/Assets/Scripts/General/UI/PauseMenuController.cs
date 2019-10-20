using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    //Public
    public GameObject m_ButtonSound;

    //Private
    private AudioSource m_AudioSource;

    private void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => RunOption(button.name));
        }
        m_AudioSource = m_ButtonSound.GetComponent<AudioSource>();
    }
    public void RunOption(string option)
    {
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
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
