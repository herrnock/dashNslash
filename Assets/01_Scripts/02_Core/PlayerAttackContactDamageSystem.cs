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
    [SerializeField] private HitFlashView hitFlashView; // Optional feedback

    private float lastDamageTime = -Mathf.Infinity;

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

        if (combatModeSystem.Mode != CombatModeSystem.CombatMode.Attack)
            return;

        if (movementSystem.State != MovementSystem.MovementState.Moving)
            return;

        HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
            lastDamageTime = Time.time;
            // Play hit flash feedback if assigned
            if (hitFlashView != null)
                hitFlashView.PlayFlash();
        }
    }
}
