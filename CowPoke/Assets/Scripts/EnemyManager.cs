using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager that updates enemies in a single loop to avoid per-enemy Update() overhead.
/// - Keep this component on a persistent GameObject in the scene (e.g. GameManager).
/// - It finds the player by tag and exposes PlayerTransform for enemies to use.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public static Transform PlayerTransform { get; set; }

    List<Enemy> enemies = new List<Enemy>(256);

    [Header("Spawning")]
    [Tooltip("Enemy prefab to spawn")] public GameObject enemyPrefab;
    [Tooltip("Minimum seconds between spawns at the start")] public float minSpawnInterval = 2.5f;
    [Tooltip("Maximum spawn rate (minimum interval) at 10 minutes")] public float maxSpawnInterval = 0.4f;
    [Tooltip("Time in seconds to reach max spawn rate")] public float timeToMaxRate = 600f; // 10 minutes
    [Tooltip("Radius around player to spawn enemies")] public float spawnRadius = 12f;
    [Tooltip("If true, spawn enemies only if player exists")] public bool requirePlayer = true;

    float spawnTimer = 0f;
    float elapsed = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) PlayerTransform = p.transform;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        elapsed += dt;

        // --- Enemy update loop ---
        for (int i = 0; i < enemies.Count; i++)
        {
            var e = enemies[i];
            if (e != null && e.gameObject.activeInHierarchy)
            {
                e.Tick(dt);
            }
        }

        // --- Spawning logic ---
        if (enemyPrefab != null && (!requirePlayer || PlayerTransform != null))
        {
            spawnTimer -= dt;
            float t = Mathf.Clamp01(elapsed / Mathf.Max(1f, timeToMaxRate));
            float interval = Mathf.Lerp(minSpawnInterval, maxSpawnInterval, t);
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = interval;
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 center = PlayerTransform != null ? PlayerTransform.position : Vector3.zero;
        // Random point on circle around player
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = center + new Vector3(circle.x, 10f, circle.y); // start above ground for raycast

        // Raycast down to find ground
        RaycastHit hit;
        float maxRaycastDistance = 30f;
        if (Physics.Raycast(pos, Vector3.down, out hit, maxRaycastDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            pos.y = hit.point.y;
        }
        else
        {
            // fallback: set to y=0 if no ground found
            pos.y = 0f;
        }

        Quaternion rot = Quaternion.identity;
        var go = Instantiate(enemyPrefab, pos, rot);
        // Optionally, register if not handled by enemy itself
        var enemy = go.GetComponent<Enemy>();
        if (enemy != null) Register(enemy);
    }

    public void Register(Enemy e)
    {
        if (e == null) return;
        if (enemies.Contains(e)) return;
        enemies.Add(e);
    }

    public void Unregister(Enemy e)
    {
        if (e == null) return;
        enemies.Remove(e);
    }
}
