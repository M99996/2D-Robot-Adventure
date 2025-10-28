using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RespawnPointController : MonoBehaviour
{
    private static RespawnPointController activePoint;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (RespawnManager.Instance == null) return;

        RespawnManager.Instance.RegisterCheckpoint(transform);

        if (activePoint != null && activePoint != this)
        {
            var oldSr = activePoint.GetComponent<SpriteRenderer>();
            if (oldSr) oldSr.color = Color.white;
        }

        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.color = new Color(1f, 0.5f, 0.9f);

        activePoint = this;
    }
}
