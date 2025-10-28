using UnityEngine;

public class TimerStartOnLoad : MonoBehaviour
{
    void Start()
    {
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.ResetTimer(0f);
            GameTimer.Instance.StartTimer();
        }
    }
}