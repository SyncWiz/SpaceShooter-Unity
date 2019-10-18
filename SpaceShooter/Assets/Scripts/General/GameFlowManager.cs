using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameFlowManager : MonoBehaviour
{
    private static GameFlowManager g_GameFlowManager;
    public static GameFlowManager Instance
    {
        get
        {
            if (!g_GameFlowManager)
            {
                g_GameFlowManager = FindObjectOfType(typeof(GameFlowManager)) as GameFlowManager;

                if (!g_GameFlowManager)
                {
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }
            return g_GameFlowManager;
        }
    }

    //Public
    public UnityEvent m_ScoreChangedEvent;

    //Private 
    private int m_Score;
    private bool m_InMainGame;
    private bool m_GamePaused;

    void Awake()
    {
        if (g_GameFlowManager == null)
        {
            DontDestroyOnLoad(this.gameObject);
            g_GameFlowManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_Score = 0;
        m_GamePaused = false;
        m_InMainGame = false;
        UIController.Instance.Show("MainMenu");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && m_InMainGame)
        {
            if(Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        UIController.Instance.HideAll();
        m_InMainGame = true;
        m_Score = 0;
        m_ScoreChangedEvent.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        m_GamePaused = true;
        UIController.Instance.Show("PauseMenu");
        FindObjectOfType<MainGameUIController>().HideAll();
    }

    public void ResumeGame()
    {
        UIController.Instance.Hide("PauseMenu");
        Time.timeScale = 1;
        m_GamePaused = false;
        FindObjectOfType<MainGameUIController>().ShowAll();
    }

    public void ExtiGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        m_InMainGame = false;
        ResumeGame();
        SceneManager.UnloadSceneAsync("MainGame");
        UIController.Instance.Show("MainMenu");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public bool IsGamePaused()
    {
        return m_GamePaused;
    }

    public void AddScorePoints(int score)
    {
        m_Score += score;
        m_ScoreChangedEvent.Invoke();
    }

    public int GetScore()
    {
        return m_Score;
    }

    public void EndGame()
    {
        ReturnToMainMenu();
        UIController.Instance.Hide("MainMenu");
        UIController.Instance.Show("DeadMenu");
    }

    public void Win()
    {
        ReturnToMainMenu();
        UIController.Instance.Hide("MainMenu");
        UIController.Instance.Show("WinMenu");
    }

    public void ReturnToMainMenuFromDeadMenu()
    {
        UIController.Instance.Show("MainMenu");
        UIController.Instance.Hide("DeadMenu");
        UIController.Instance.Hide("WinMenu");
    }
}
