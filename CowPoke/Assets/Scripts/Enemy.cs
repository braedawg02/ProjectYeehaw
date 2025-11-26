using UnityEngine;

/// <summary>
/// Simple enemy that moves toward the player and has a Health component.
/// - Set the player's tag (default "Player") to find the target.
/// - Uses Rigidbody.velocity if present, otherwise moves transform.
/// - Stops when it reaches closeDistance.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [Tooltip("Tag used to find the player GameObject.")]
    public string playerTag = "Player";

    [Tooltip("Movement speed in units per second.")]
    public float speed = 2f;

    [Tooltip("Distance at which the enemy will stop moving toward player.")]
    public float closeDistance = 0.6f;

    [Tooltip("Use Rigidbody for movement if present.")]
    public bool useRigidbody = true;

    Transform player;
    Rigidbody rb;
    [Header("Debug (editor)")]
    [Tooltip("Enable debug logging and gizmos for this enemy.")]
    public bool debug = false;
    [Tooltip("Color used to draw health gizmo.")]
    public Color healthColor = Color.red;

    // health tracking for debug logs
    int cachedHealth = int.MinValue;

    void Awake()
    {
        // We'll let the EnemyManager provide the player transform to avoid each enemy doing a Find.
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Register with manager so the manager can tick this enemy in a batch loop
        EnemyManager.Instance?.Register(this);
    }

    void OnDisable()
    {
        EnemyManager.Instance?.Unregister(this);
    }

    void Start()
    {
        // In some cases (order of initialization) the manager may not exist yet when OnEnable ran.
        // Try registering again in Start to be safe.
        EnemyManager.Instance?.Register(this);
    }

    /// <summary>
    /// Called by EnemyManager each frame to perform movement. This avoids a per-enemy Update() call.
    /// </summary>
    public void Tick(float deltaTime)
    {
        var playerTransform = EnemyManager.PlayerTransform;
        if (playerTransform == null)
        {
            if (debug) Debug.LogWarning($"[Enemy] '{gameObject.name}' - No player transform found from EnemyManager!");
            return;
        }

        if (debug) Debug.Log($"[Enemy] '{gameObject.name}' - Player found at {playerTransform.position}, Enemy at {transform.position}");


        Vector3 toPlayer = playerTransform.position - transform.position;
        toPlayer.y = 0f; // keep movement on XZ plane
        float dist = toPlayer.magnitude;

        if (debug) Debug.Log($"[Enemy] '{gameObject.name}' - Distance to player: {dist:F2}, Close distance: {closeDistance}");

        // Always face the player on the XZ plane, even when close
        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            Vector3 lookDir = toPlayer.normalized;
            // Only rotate on Y axis
            Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.35f);
        }

        if (dist <= closeDistance)
        {
            // reached
            if (debug) Debug.Log($"[Enemy] '{gameObject.name}' - Reached player, stopping movement");
            if (rb != null && useRigidbody)
            {
                rb.linearVelocity = Vector3.zero;
            }
            return;
        }

        Vector3 dir = toPlayer.normalized;
        Vector3 vel = dir * speed;

        if (debug) Debug.Log($"[Enemy] '{gameObject.name}' - Moving toward player. Direction: {dir}, Velocity: {vel}, UseRigidbody: {useRigidbody}");

        if (rb != null && useRigidbody)
        {
            rb.linearVelocity = vel;
            if (debug) Debug.Log($"[Enemy] '{gameObject.name}' - Set Rigidbody velocity to {vel}");
        }
        else
        {
            Vector3 newPos = transform.position + vel * deltaTime;
            if (debug) Debug.Log($"[Enemy] '{gameObject.name}' - Moving transform from {transform.position} to {newPos}");
            transform.position = newPos;
        }

        // Debug: check and log health changes
        if (debug)
        {
            var h = GetComponent<Health>();
            if (h != null)
            {
                if (h.currentHealth != cachedHealth)
                {
                    Debug.Log($"[Enemy] '{gameObject.name}' health changed: {cachedHealth} -> {h.currentHealth}");
                    cachedHealth = h.currentHealth;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!debug) return;
        
        // Draw health bar
        var h = GetComponent<Health>();
        if (h != null)
        {
            Vector3 pos = transform.position + Vector3.up * 1.2f;
            float width = 0.6f;
            float filled = Mathf.Clamp01((float)h.currentHealth / Mathf.Max(1, h.maxHealth));

            // background
            Gizmos.color = Color.black;
            Gizmos.DrawCube(pos, new Vector3(width, 0.08f, 0.01f));
            // foreground
            Gizmos.color = healthColor;
            Vector3 left = pos - Vector3.right * (width * 0.5f);
            Vector3 filledCenter = left + Vector3.right * (width * filled * 0.5f);
            Gizmos.DrawCube(filledCenter, new Vector3(width * filled, 0.06f, 0.01f));
        }

        // Draw pathfinding debug info
        var playerTransform = EnemyManager.PlayerTransform;
        if (playerTransform != null)
        {
            // Draw line to player
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, playerTransform.position);
            
            // Draw movement direction
            Vector3 toPlayer = playerTransform.position - transform.position;
            toPlayer.y = 0f;
            if (toPlayer.magnitude > closeDistance)
            {
                Vector3 dir = toPlayer.normalized;
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, dir * 2f);
            }
            
            // Draw close distance radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closeDistance);
        }
    }

    void OnDrawGizmos()
    {
        if (!debug) return;
        
        // Always show enemy position and forward direction
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * 1f);
    }
}
