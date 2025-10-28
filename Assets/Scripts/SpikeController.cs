using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpikeController : MonoBehaviour
{
    [SerializeField] private Collider2D hitbox; 

    private void Reset()
    {
        var collider = GetComponent<Collider2D>();
        if (collider) collider.isTrigger = true;
        if (!hitbox) hitbox = collider;
    }

    private void Awake() { if (!hitbox) hitbox = GetComponent<Collider2D>();}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!hitbox || !hitbox.enabled) return;
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    // Animation events
    public void HitboxOn()  { if (hitbox) hitbox.enabled = true; }
    public void HitboxOff() { if (hitbox) hitbox.enabled = false; }
}