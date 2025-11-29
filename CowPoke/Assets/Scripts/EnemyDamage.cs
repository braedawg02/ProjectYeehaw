using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 999;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null && !ph.IsDead)
            {
                ph.TakeDamage(damage);
            }
        }
    }
}
