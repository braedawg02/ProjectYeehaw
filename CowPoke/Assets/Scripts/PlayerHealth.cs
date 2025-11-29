using UnityEngine;
using System.Collections;

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

    public PlayerRagdoll ragdoll;

    void Awake()
    {
        if (currentHealth < 0) currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        SoundManager.PlaySound(SoundType.Hurt);

        if (currentHealth <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }


    // ⭐ DEATH SEQUENCE WITH RAGDOLL + DELAY ⭐
    private IEnumerator DeathSequence()
    {
        Die();

        // Give ragdoll time to fall (physics-only mode)
        yield return new WaitForSeconds(1.5f);

        // Show Game Over UI
        GameOverPanel.SetActive(true);

        // OPTIONAL: freeze game AFTER ragdoll finishes
        Time.timeScale = 0f;
    }


    public void Die()
    {
        SoundManager.PlaySound(SoundType.Death);

        if (ragdoll != null)
            ragdoll.EnableRagdoll();

        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        if (destroyOnDeath)
        {
            Destroy(gameObject, 3f); // allow ragdoll to settle
        }
    }
}

