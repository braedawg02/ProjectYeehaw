using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Maximum hit points.")]
    public IntData maxHealth;

    [Tooltip("Current hit points (for debugging you can set this in inspector).")]
    public IntData currentHealth;

    [Tooltip("If true the GameObject will be destroyed when health reaches zero.")]
    public bool destroyOnDeath = true;

    [Tooltip("Optional effect prefab to spawn when this object dies.")]
    public GameObject deathEffectPrefab;

    public bool IsDead => currentHealth.value <= 0;

    public GameObject GameOverPanel;

    public PlayerRagdoll ragdoll;
    
    public Image healthBar;

    void Awake()
    {
        if (currentHealth.value <= 0) currentHealth.value = maxHealth.value;
        healthBar.fillAmount = 1f;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth.value -= amount;
        SoundManager.PlaySound(SoundType.Hurt);

        if (currentHealth.value <= 0)
        {
            StartCoroutine(DeathSequence());
        }
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        currentHealth.value = Mathf.Min(maxHealth.value, currentHealth.value + amount);
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
    
    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth.value / maxHealth.value;
        }
    }
}

