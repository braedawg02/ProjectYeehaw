using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private Rigidbody[] bodies;
    private Collider[] colliders;

    public Animator animator;
    public CharacterController characterController;

    void Awake()
    {
        bodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        // Turn off animation + player movement controller
        if (animator) animator.enabled = false;
        if (characterController) characterController.enabled = false;

        foreach (var rb in bodies)
        {
            rb.isKinematic = false;
        }

        foreach (var col in colliders)
        {
            // Don’t touch the main root collider (CharacterController)
            if (col.transform != transform)
                col.enabled = true;
        }
    }

    public void DisableRagdoll()
    {
        foreach (var rb in bodies)
        {
            rb.isKinematic = true;
        }

        foreach (var col in colliders)
        {
            if (col.transform != transform)
                col.enabled = false;
        }
    }
}
