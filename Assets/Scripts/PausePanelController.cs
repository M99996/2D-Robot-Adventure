using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PausePanelController : MonoBehaviour
{
    [Header("References")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    public Button exitButton;
    public string mainMenuSceneName = "StartMenu";
    public string firstLevelSceneName = "MainScene";
    public PlayerController player;

    bool paused = false;
    bool busyLoading = false;

    void Awake()
    {
        SetVisible(false);
        if (resumeButton) resumeButton.onClick.AddListener(Resume);
        if (restartButton) restartButton.onClick.AddListener(Restart);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(ReturnToMenu);
        if (exitButton) exitButton.onClick.AddListener(ExitGame);
    }

    void SetVisible(bool v)
    {
        if (pausePanel) pausePanel.SetActive(v);
        if (pausePanel) pausePanel.transform.SetAsLastSibling();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) Pause();
            else Resume();
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        if (player) player.canControl = false;
        if (resumeButton) resumeButton.Select();
        SetVisible(true);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        if (player) player.canControl = true;
        SetVisible(false);
    }

    void Restart()
    {
        if (busyLoading) return;
        busyLoading = true;

        paused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (player) player.canControl = false;
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.ResetTimer(0f);
            GameTimer.Instance.Stop();
        }

        SceneManager.LoadScene(firstLevelSceneName);
    }

    void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}