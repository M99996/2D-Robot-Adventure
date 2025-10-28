using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularController : MonoBehaviour
{
    [SerializeField] private Collider2D hitbox;

    public bool large = false;

    private void Reset()
    {
        var collider = GetComponent<Collider2D>();
        if (collider) collider.isTrigger = true;
        if (!hitbox) hitbox = collider;
    }

    private void Awake() { if (!hitbox) hitbox = GetComponent<Collider2D>(); }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!hitbox || !hitbox.enabled) return;
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {   
            if (large) player.ChangeHealth(-9999);
            else player.ChangeHealth(-2);
        }
    }
}
