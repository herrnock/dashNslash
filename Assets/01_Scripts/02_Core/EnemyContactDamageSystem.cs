using UnityEngine;

/// Handles enemy contact damage to the player using 2D trigger events and cooldown.
public class EnemyContactDamageSystem : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float damageCooldown = 1f;

    [Header("References")]
    [SerializeField] private CombatModeSystem combatModeSystem;
    [SerializeField] private ShieldSystem shieldSystem;
    [SerializeField] private BlockFeedbackView blockFeedbackView; // Optional: plays shield absorb feedback
    [SerializeField] private MovementSystem movementSystem; // Reference to player movement system

    private float lastDamageTime = -Mathf.Infinity;

    private bool movementWarningLogged = false;
    private bool combatModeWarningLogged = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryApplyDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryApplyDamage(other);
    }

    private void TryApplyDamage(Collider2D other)
    {
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        HealthSystem playerHealth = other.GetComponent<HealthSystem>();
        if (playerHealth == null)
            return;


        // --- Stricter Attack Dash Ignore Logic ---
        if (combatModeSystem == null && !combatModeWarningLogged)
        {
            Debug.LogWarning($"[EnemyContactDamageSystem] CombatModeSystem reference missing on {gameObject.name}. Cannot check for Attack Mode.", this);
            combatModeWarningLogged = true;
        }
        if (movementSystem == null && !movementWarningLogged)
        {
            Debug.LogWarning($"[EnemyContactDamageSystem] MovementSystem reference missing on {gameObject.name}. Cannot check for Moving state.", this);
            movementWarningLogged = true;
        }
        bool validAttackDash = false;
        if (combatModeSystem != null && movementSystem != null)
        {
            if (combatModeSystem.Mode == CombatModeSystem.CombatMode.Attack &&
                movementSystem.State == MovementSystem.MovementState.Moving &&
                movementSystem.IsActivelyMoving)
            {
                // Direction check: player must be moving toward this enemy
                Vector2 playerPos = movementSystem.transform.position;
                Vector2 enemyPos = transform.position;
                Vector2 toEnemy = (enemyPos - playerPos).normalized;
                Vector2 moveDir = movementSystem.LastMoveDirection;
                if (moveDir != Vector2.zero && Vector2.Dot(moveDir, toEnemy) > 0.25f)
                {
                    validAttackDash = true;
                }
            }
        }
        if (validAttackDash)
        {
            // Valid Attack Dash: skip normal contact damage, do not set cooldown
            return;
        }
        // --- End Stricter Attack Dash Ignore Logic ---

        // Check player mode and shield
        if (combatModeSystem != null && shieldSystem != null)
        {
            if (combatModeSystem.Mode == CombatModeSystem.CombatMode.Defence)
            {
                bool shielded = shieldSystem.ConsumeCharge();
                if (shielded)
                {
                    // Play shield absorb feedback if assigned
                    if (blockFeedbackView != null)
                        blockFeedbackView.PlayBlockFeedback();
                    // Shield absorbed the hit, check for break
                    if (shieldSystem.IsBroken)
                    {
                        // Force Attack Mode on shield break
                        combatModeSystem.SetAttackMode();
                    }
                    lastDamageTime = Time.time;
                    return;
                }
                // else: shield empty, fall through to health damage
            }
        }

        // Attack Mode or shield empty: apply health damage
        playerHealth.TakeDamage(damageAmount);
        lastDamageTime = Time.time;

        // Log warnings if not already logged
        if (!movementWarningLogged)
        {
            movementWarningLogged = true;
            Debug.LogWarning("Player is moving during enemy contact damage. This may be a bug.");
        }

        if (!combatModeWarningLogged)
        {
            combatModeWarningLogged = true;
            Debug.LogWarning("Player is in Defence mode during enemy contact damage. This may be a bug.");
        }
    }
}
