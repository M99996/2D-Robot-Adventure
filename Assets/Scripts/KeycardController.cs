using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KeycardController : MonoBehaviour
{
    public float followSpeed = 6f;
    public Vector2 followOffset = new Vector2(-0.6f, 0.1f);
    public float bobAmplitude = 0.001f;
    public float bobSpeed = 3f;

    private bool collected = false;
    private Transform player;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;
    private Vector3 idleBasePos;
    private Collider2D col;
    private SpriteRenderer sr;

    public bool IsCollected{ get { return collected; }}

    private void Awake()
    {
        idleBasePos = transform.position;
        col = GetComponent<Collider2D>();
        sr  = GetComponent<SpriteRenderer>();
    }

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = true;
    }

    private void Update()
    {
        if (!collected)
        {
            float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            transform.position = new Vector3(idleBasePos.x, idleBasePos.y + bob, idleBasePos.z);
            return;
        }

        if (player == null) return;

        bool facingRight = GetFacingRight();

        float xOffset = Mathf.Abs(followOffset.x) * (facingRight ? -1f : 1f);
        Vector3 targetPos = player.position + new Vector3(xOffset, followOffset.y, 0f);

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        float bob2 = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        transform.position += new Vector3(0f, bob2, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        player = other.transform;
        playerRb = other.attachedRigidbody;
        playerSprite = other.GetComponentInChildren<SpriteRenderer>();

        collected = true;
        if (col) col.enabled = false;
    }

    bool GetFacingRight()
    {
        if (playerSprite != null)
        {
            return !playerSprite.flipX;
        }
        if (playerRb != null)
        {
            if (playerRb.velocity.x > 0.05f) return true;
            if (playerRb.velocity.x < -0.05f) return false;
        }
        return true;
    }
}
