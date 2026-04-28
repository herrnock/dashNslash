using UnityEngine;

/// Simple enemy system: follows the player on the XY plane.
public class EnemySystem : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private EnemyStunSystem enemyStunSystem; // Optional: controls stun state

    private void Update()
    {
        if (playerTarget == null)
            return;

        // If stunned, skip movement
        if (enemyStunSystem != null && enemyStunSystem.IsStunned)
            return;

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(playerTarget.position.x, playerTarget.position.y);
        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance > stopDistance)
        {
            Vector2 direction = (targetPosition - currentPosition).normalized;
            Vector2 movement = direction * moveSpeed * Time.deltaTime;
            // Clamp movement so we don't overshoot
            if (movement.magnitude > distance - stopDistance)
                movement = direction * (distance - stopDistance);
            Vector2 newPosition = currentPosition + movement;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
        // else: within stopDistance, do nothing (idle)
    }
}
