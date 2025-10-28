using UnityEngine;

public class HookController : MonoBehaviour
{
    [Header("References")]
    public Transform hookOrigin;
    public LineRenderer line;
    public SpriteRenderer hookHead;
    public Rigidbody2D playerRb;
    public DistanceJoint2D joint;
    public LayerMask groundMask;

    [Header("Lengths")]
    public float maxLength = 5f;
    public float minLength = 1.2f;
    public float shootSpeed = 40f;
    public float shortenSpeed = 10f;
    public float lengthenSpeed = 10f;

    [Header("Input")]
    public KeyCode fireKey = KeyCode.E;
    public KeyCode shortenKey = KeyCode.W;
    public KeyCode lengthenKey = KeyCode.S;
    public enum State { Idle, Firing, Latched }

    private State state = State.Idle;
    private Vector2 fireDirection;
    private Vector2 hitPoint;
    private float currentLength = 0f;

    public State CurrentState { get; private set; } = State.Idle;

    void Awake()
    {
        if (line)
        {
            line.enabled = false;
            line.positionCount = 2;
            line.textureMode = LineTextureMode.Stretch;
        }
        if (hookHead) hookHead.enabled = false;
        if (joint) joint.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(fireKey))
        {
            if (state == State.Latched) Release();
            else if (state == State.Idle) BeginFire();
        }

        if (state == State.Latched && Input.GetKeyDown(KeyCode.Space))
        {
            Release();
        }

        switch (state)
        {
            case State.Firing: TickFiring(); break;
            case State.Latched: TickLatched(); break;
        }

        UpdateVisuals();
    }

    void BeginFire()
    {
        Vector3 m = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m.z = 0;
        fireDirection = (m - hookOrigin.position).normalized;

        currentLength = 0f;
        state = State.Firing;

        if (line) line.enabled = true;
        if (hookHead)
        {
            hookHead.enabled = true;
            hookHead.transform.position = hookOrigin.position;
        }
        if (joint) joint.enabled = false;
    }

    void TickFiring()
    {
        currentLength = Mathf.MoveTowards(currentLength, maxLength, shootSpeed * Time.deltaTime);

        // check if hit ground layer
        RaycastHit2D hit = Physics2D.Raycast(hookOrigin.position, fireDirection, currentLength, groundMask);
        if (hit.collider != null)
        {
            hitPoint = hit.point;
            currentLength = Vector2.Distance(playerRb.position, hitPoint);

            if (joint)
            {
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = hitPoint;
                joint.autoConfigureDistance = false;
                joint.distance = currentLength;
                joint.enabled = true;
            }

            state = State.Latched;
            return;
        }

        // Return if nothing hit
        if (Mathf.Approximately(currentLength, maxLength))
        {
            Release();
        }
    }

    void TickLatched()
    {
        if (Input.GetKey(shortenKey))
        {
            currentLength = Mathf.Max(minLength, currentLength - shortenSpeed * Time.deltaTime);
            if (joint) joint.distance = currentLength;
        }
        else if (Input.GetKey(lengthenKey))
        {
            currentLength = Mathf.Min(maxLength, currentLength + lengthenSpeed * Time.deltaTime);
            if (joint) joint.distance = currentLength;
        }
    }

    // Disable all components of hook
    public void Release()
    {
        state = State.Idle;
        if (joint) joint.enabled = false;
        if (line) line.enabled = false;
        if (hookHead) hookHead.enabled = false;
    }
    
    void UpdateVisuals()
    {
        if (state == State.Idle) return;

        Vector3 origin = hookOrigin.position;
        Vector3 end = (state == State.Firing)
            ? origin + (Vector3)(fireDirection * currentLength)
            : (Vector3)hitPoint;

        if (line)
        {
            line.SetPosition(0, origin);
            line.SetPosition(1, end);
        }

        if (hookHead)
        {
            hookHead.enabled = true;
            hookHead.transform.position = end;

            Vector2 dir = (end - origin).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            hookHead.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}