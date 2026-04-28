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

        HealthSystem playerHealth = other.GetComponent<HealthSystem>();
        if (playerHealth == null)
            return;

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
    }
}
