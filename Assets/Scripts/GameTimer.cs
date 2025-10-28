using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    private float elapsed = 0f;
    private bool running = false;

    public float GetTime() => elapsed;
    public void Stop() => running = false;
    public void StartTimer() => running = true;
    public void ResetTimer(float startAt = 0f) { elapsed = startAt; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        elapsed = 0f;
        running = true;
    }

    void Update()
    {
        if (running) elapsed += Time.deltaTime;
    }

    public string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        float seconds = t - minutes * 60f;
        return $"{minutes:00}:{seconds:00.000}";
    }
}