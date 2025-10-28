using UnityEngine;
using UnityEngine.UI;

public class HealthBarContoller : MonoBehaviour
{
    [Header("Refs")]
    public PlayerController player;
    public Image fillImage;

    [Header("Visual")]
    public float lerpSpeed = 8f;

    void Start()
    {
        if (!player) player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!player || !fillImage) return;

        float target = Mathf.Clamp01((float)player.currentHealth / Mathf.Max(1, player.maxHealth));

        fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, target, lerpSpeed * Time.deltaTime);
    }
}
