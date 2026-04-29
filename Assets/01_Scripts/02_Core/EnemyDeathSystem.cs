using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyDeathSystem : MonoBehaviour
{
    private HealthSystem healthSystem;
    private bool hasDied = false;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem == null)
        {
            Debug.LogError($"[EnemyDeathSystem] No HealthSystem found on {gameObject.name}. Disabling EnemyDeathSystem.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (hasDied || healthSystem == null)
            return;

        if (healthSystem.IsDead)
        {
            hasDied = true;
            Destroy(gameObject);
        }
    }
}
