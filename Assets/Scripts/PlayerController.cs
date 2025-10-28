using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;
    public bool isDead = false;
    public bool canControl = true;
    public bool isInvincible = false;
    public float timeInvincible = 1.5f;
    [HideInInspector] public float damageCooldown;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Hook")]
    public HookController hook;

    // Player components
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {   
        if(!animator) animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead || !canControl) return;

        // Check if grounded
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        }

        // Invincibility
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown <= 0)
            {
                isInvincible = false;
            }
        }

        // Jump
        if ((isGrounded || hook.CurrentState == HookController.State.Latched) && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Facing
        float x = Input.GetAxisRaw("Horizontal");
        if (sr && Mathf.Abs(x) > 0.01f)
            sr.flipX = x < 0;
    }

    void FixedUpdate()
    {
        if (isDead || !canControl)
        {
            if (animator) animator.SetFloat("speed", 0f);
            return;
        }

        // Horizontal movement
        float x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        float spd = Mathf.Abs(x);
    if (animator) animator.SetFloat("speed", spd);
    }

    public void ChangeHealth(int amount)
    {
        if (amount == 0) return;
        if (amount < 0)
        {
            if (isInvincible || isDead)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;

            StartCoroutine(BlinkEffect());
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (!isDead && currentHealth == 0) { PlayerRespawn(); }
    }
    
    public void PlayerRespawn()
    {
        if (hook != null) hook.Release();

        isDead = true;
        if (RespawnManager.Instance != null)
            RespawnManager.Instance.Respawn();
        else
            Debug.LogWarning("RespawnManager not found.");
    }

    // Show area of groundcheck in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // Dmaaged effort
    private IEnumerator BlinkEffect()
    {
        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        sr.enabled = true;
    }
}