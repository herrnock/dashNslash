using UnityEngine;

/// <summary>
/// Handles player damaging enemies on contact, only in Attack Mode and while Moving.
/// </summary>
public class PlayerAttackContactDamageSystem : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private CombatModeSystem combatModeSystem;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private HealthSystem playerHealthSystem;

    private float lastDamageTime = -Mathf.Infinity;
    private bool playerHealthWarningLogged = false;

    private void Awake()
    {
        if (playerHealthSystem == null)
        {
            playerHealthSystem = GetComponent<HealthSystem>();
            if (playerHealthSystem == null && !playerHealthWarningLogged)
            {
                Debug.LogWarning($"[PlayerAttackContactDamageSystem] No player HealthSystem assigned or found on {gameObject.name}.", this);
                playerHealthWarningLogged = true;
            }
        }
    }

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

        if (combatModeSystem == null || movementSystem == null)
            return;

        // Strict valid Attack Dash condition
        if (combatModeSystem.Mode != CombatModeSystem.CombatMode.Attack)
            return;
        if (movementSystem.State != MovementSystem.MovementState.Moving)
            return;
        if (!movementSystem.IsActivelyMoving)
            return;

        // Find enemy HealthSystem on root or parent
        HealthSystem enemyHealth = other.GetComponentInParent<HealthSystem>();
        if (enemyHealth == null)
            return;

        // Direction check: player must be moving toward enemy

        UnityEngine.Vector3 playerPos = transform.position;
        UnityEngine.Vector3 enemyPos = enemyHealth.transform.position;
        Vector2 toEnemy = (enemyPos - playerPos).normalized;
        Vector2 moveDir = movementSystem.LastMoveDirection;
        if (moveDir == Vector2.zero)
            return;
        float dot = Vector2.Dot(moveDir, toEnemy);
        if (dot <= 0.25f)
            return;

        // Guard check
        EnemyGuardSystem guardSystem = enemyHealth.GetComponent<EnemyGuardSystem>();
        if (guardSystem != null && guardSystem.BlocksAttackFrom(playerPos))
        {
            // Guard blocks: apply counter damage to player
            if (playerHealthSystem != null)
            {
                playerHealthSystem.TakeDamage(guardSystem.CounterDamageOnAttackDash);
            }
            lastDamageTime = Time.time;
            return;
        }

        // No guard block: damage enemy
        enemyHealth.TakeDamage(attackDamage);
        lastDamageTime = Time.time;

        // Play enemy hit flash if present
        HitFlashView hitFlash = enemyHealth.GetComponentInChildren<HitFlashView>();
        if (hitFlash != null)
            hitFlash.PlayFlash();
    }
}
