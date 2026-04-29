using UnityEngine;

// Click-to-move Logik für ein 2D-Objekt auf der XY-Ebene
public class MovementSystem : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Moving
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 0.1f;

    private Vector2 targetPosition;
    private bool hasTarget = false;
    private MovementState currentState = MovementState.Idle;

    // --- New movement info ---
    public bool IsActivelyMoving { get; private set; }
    public Vector2 LastMoveDelta { get; private set; }
    public Vector2 LastMoveDirection { get; private set; }

    public MovementState State => currentState;

    public void SetTarget(Vector2 position)
    {
        // If already at target, do not start movement
        if (Vector2.Distance((Vector2)transform.position, position) <= stopDistance)
        {
            hasTarget = false;
            currentState = MovementState.Idle;
            return;
        }
        targetPosition = position;
        hasTarget = true;
        currentState = MovementState.Moving;
    }

    private void Update()
    {
        // Reset movement info at start of frame
        IsActivelyMoving = false;
        LastMoveDelta = Vector2.zero;

        if (!hasTarget)
        {
            currentState = MovementState.Idle;
            LastMoveDirection = Vector2.zero;
            return;
        }

        Vector2 currentPosition = transform.position;
        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance <= stopDistance)
        {
            hasTarget = false;
            currentState = MovementState.Idle;
            LastMoveDirection = Vector2.zero;
            return;
        }

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        Vector2 delta = newPosition - currentPosition;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        currentState = MovementState.Moving;

        if (delta.magnitude > 0.001f)
        {
            IsActivelyMoving = true;
            LastMoveDelta = delta;
            LastMoveDirection = delta.normalized;
        }
        else
        {
            IsActivelyMoving = false;
            LastMoveDelta = Vector2.zero;
        }
    }
}
