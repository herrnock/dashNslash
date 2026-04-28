using UnityEngine;

/// <summary>
/// Handles knockback and/or stun of enemies when the player dashes (moves) in Defence Mode and contacts an enemy.
/// </summary>
public class DefenceDashContactSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private CombatModeSystem combatModeSystem;

    [Header("Defence Dash Settings")]
    [SerializeField] private bool applyKnockback = true;
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private bool applyStun = true;
    [SerializeField] private float enemyStunDuration = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger if in Defence Mode and Moving
        if (combatModeSystem == null || movementSystem == null)
            return;
        if (combatModeSystem.Mode != CombatModeSystem.CombatMode.Defence)
            return;
        if (movementSystem.State != MovementSystem.MovementState.Moving)
            return;

        // Knockback
        if (applyKnockback)
        {
            Transform enemyTransform = other.transform;
            Vector2 direction = (enemyTransform.position - transform.position).normalized;
            enemyTransform.position += (Vector3)(direction * knockbackForce);
        }

        // Stun
        if (applyStun)
        {
            EnemyStunSystem stunSystem = other.GetComponent<EnemyStunSystem>();
            if (stunSystem != null)
            {
                stunSystem.ApplyStun(enemyStunDuration);
            }
        }
    }
}
