using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //Public
    public GameObject m_ButtonSound;

    //Private
    private Button[] m_Buttons;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_Buttons = GetComponentsInChildren<Button>();
        foreach (Button button in m_Buttons)
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
