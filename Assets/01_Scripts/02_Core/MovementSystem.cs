using UnityEngine;

// Click-to-move Logik für ein 2D-Objekt auf der XY-Ebene
public class MovementSystem : MonoBehaviour
{
    // Enum für aktuellen Bewegungszustand
    public enum MovementState
    {
        Idle,
        Moving
    }

    [SerializeField] private float moveSpeed = 5f; // Movement speed in units per second
    [SerializeField] private float stopDistance = 0.1f; // Distance threshold to stop at target

    private Vector2 targetPosition; // Last movement target
    private bool hasTarget = false; // Is a target currently set?

    private MovementState currentState = MovementState.Idle; // Current movement state

    // Gibt aktuellen Bewegungszustand zurück (nur lesbar)
    public MovementState State => currentState;

    // Setzt ein neues Bewegungsziel und startet die Bewegung
    public void SetTarget(Vector2 position)
    {
        targetPosition = position;
        hasTarget = true;
        currentState = MovementState.Moving;
    }

    // Handles movement towards the target each frame
    private void Update()
    {
        if (!hasTarget)
        {
            currentState = MovementState.Idle;
            return;
        }

        Vector2 currentPosition = transform.position;
        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance <= stopDistance)
        {
            hasTarget = false;
            currentState = MovementState.Idle;
            return;
        }

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        currentState = MovementState.Moving;
    }
}
