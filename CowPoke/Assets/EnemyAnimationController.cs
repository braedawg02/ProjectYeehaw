using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    [Header("Animation Events")]
    public UnityEvent onStepEvent; // optional event for footsteps, attacks, etc.

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Example: Start with walking animation
        animator.SetBool("IsWalking", true);
    }

    // Example: called from animation event in the Walk clip
    public void Step()
    {
        onStepEvent?.Invoke();
    }
}
