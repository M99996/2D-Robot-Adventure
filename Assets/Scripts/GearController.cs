using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GearController : MonoBehaviour
{
    [Header("Heal")]
    public int healAmount = 1;
    [Header("Animation")]
    public bool bobbing = true;
    public float bobAmplitude = 0.06f;
    public float bobSpeed = 3f;

    private Vector3 basePos;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = true;
    }

    private void Awake()
    {
        basePos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (bobbing)
        {
            float o = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
            transform.position = new Vector3(basePos.x, basePos.y + o, basePos.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller == null || controller.isDead) return;

        if (controller.currentHealth < controller.maxHealth)
        {
            controller.ChangeHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
