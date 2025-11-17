using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public VariableJoystick joystick; // 🎮 Add this line!

    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    private bool isMoving = false;

    [Header("Animation Events")]
    public UnityEvent walkEvent;
    public UnityEvent idleEvent;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // ✅ Combine keyboard and joystick input
        float moveX = (joystick != null && Mathf.Abs(joystick.Horizontal) > 0.01f)
            ? joystick.Horizontal
            : Input.GetAxis("Horizontal");

        float moveZ = (joystick != null && Mathf.Abs(joystick.Vertical) > 0.01f)
            ? joystick.Vertical
            : Input.GetAxis("Vertical");

        // Movement direction
        moveDirection = new Vector3(moveX, 0, moveZ);

        // Normalize diagonal movement
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        // Move the player
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotate the player when moving
        if (moveDirection.magnitude > 0.05f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }

        // Handle animation events
        if (moveDirection.magnitude > 0.05f)
        {
            if (!isMoving)
            {
                isMoving = true;
                walkEvent.Invoke();
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                idleEvent.Invoke();
            }
        }
    }
}