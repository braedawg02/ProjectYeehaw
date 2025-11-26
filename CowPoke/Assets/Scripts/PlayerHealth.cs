using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Maximum hit points.")]
    public int maxHealth = 3;

    [Tooltip("Current hit points (for debugging you can set this in inspector).")]
    public int currentHealth = -1;

    [Tooltip("If true the GameObject will be destroyed when health reaches zero.")]
    public bool destroyOnDeath = true;

    [Tooltip("Optional effect prefab to spawn when this object dies.")]
    public GameObject deathEffectPrefab;

    public bool IsDead => currentHealth <= 0;

    public GameObject GameOverPanel;
    
    void Awake()
    {
        if (currentHealth < 0) currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage. This method signature is compatible with SendMessage from Projectile.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        currentHealth -= amount;
        SoundManager.PlaySound(SoundType.Hurt);
        if (currentHealth <= 0)
        {
            Die();
            Time.timeScale = 0f;
            GameOverPanel.SetActive(true);
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void Die()
    {
        SoundManager.PlaySound(SoundType.Death);
        // spawn effect if provided
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // optionally destroy
        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
