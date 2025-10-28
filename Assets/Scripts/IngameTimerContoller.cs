using UnityEngine;
using TMPro;

public class IngameTimerController : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Start()
    {
        if (!timeText) timeText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Reset()
    {
        if (!timeText) timeText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!timeText) return;

        if (GameTimer.Instance == null)
        {
            timeText.text = "00:00.000";
            return;
        }

        float t = GameTimer.Instance.GetTime();
        timeText.text = GameTimer.Instance.FormatTime(t);
    }
}
