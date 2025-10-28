using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [Header("References")]
    public PlayerController player;
    public Transform startPoint;

    [Header("Timing")]
    public float respawnDelay = 0.25f;
    public float spawnInvincibleTime = 0.5f;

    private Transform currentCheckpoint;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        currentCheckpoint = startPoint;
    }

    public void RegisterCheckpoint(Transform cp)
    {
        currentCheckpoint = cp;
        Debug.Log("Checkpoint registered: " + cp.name);
    }

    public void Respawn()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        if (player == null) yield break;

        player.isDead = true;
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb) rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(respawnDelay);

        var target = currentCheckpoint != null ? currentCheckpoint : startPoint;
        if (target) player.transform.position = target.position;
        if (rb) rb.velocity = Vector2.zero;

        player.currentHealth = player.maxHealth;
        player.isDead = false;
        player.isInvincible = true;
        player.damageCooldown = spawnInvincibleTime;
    }
}