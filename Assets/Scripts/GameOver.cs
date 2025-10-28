using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI timeText;
    public Button restartButton;
    public Button mainMenuButton;
    public PlayerController player;
    public string firstLevelSceneName = "MainScene";
    private string startMenu = "StartMenu";

    bool busyLoading = false;

    void Awake()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (restartButton) restartButton.onClick.AddListener(Restart);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(ReturnToMenu);
    }

    public void ShowGameOver()
    {
        // Stop the timer
        if (GameTimer.Instance) GameTimer.Instance.Stop();

        // Disable player control
        if (player)
        {
            player.canControl = false;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) rb.velocity = Vector2.zero;
        }

        float t = GameTimer.Instance ? GameTimer.Instance.GetTime() : Time.timeSinceLevelLoad;
        string nickname = PlayerPrefs.GetString("player_nickname", "PLAYER");
        LeaderboardManager.AddEntry(nickname, t);

        // Edit timer display
        if (timeText)
        {
            timeText.text = "Time: " + GameTimer.Instance.FormatTime(t);
        }

        // display panel
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    void Restart()
    {
        if (busyLoading) return;
        busyLoading = true;


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
        if (player) player.canControl = true;
        SceneManager.LoadScene(startMenu);
    }
}