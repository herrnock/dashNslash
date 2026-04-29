using UnityEngine;

public class EnemyGuardSystem : MonoBehaviour
{
    public enum GuardSide
    {
        None,
        Left,
        Right
    }

    [SerializeField] private bool guardEnabled = true;
    [SerializeField] private float guardDuration = 1.0f;
    [SerializeField] private float guardCooldown = 3.0f;
    [SerializeField] private float counterDamageOnAttackDash = 1.0f;
    [SerializeField] private bool blockAttackDashDamage = true;
    [SerializeField] private bool startWithGuardReady = true;
    [SerializeField] private Transform playerTransform;

    public bool GuardEnabled => guardEnabled;
    public float CounterDamageOnAttackDash => counterDamageOnAttackDash;
    public bool BlockAttackDashDamage => blockAttackDashDamage;
    public bool IsGuardActive => (stunSystem != null && stunSystem.IsStunned) ? false : isGuardActive;
    public GuardSide CurrentGuardSide => (stunSystem != null && stunSystem.IsStunned) ? GuardSide.None : currentGuardSide;
    public bool HasDirectionalGuard => IsGuardActive && CurrentGuardSide != GuardSide.None;

    private float timer;
    private EnemyStunSystem stunSystem;
    private bool isGuardActive;
    private GuardSide currentGuardSide;

    private void Awake()
    {
        stunSystem = GetComponent<EnemyStunSystem>();
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogWarning($"[EnemyGuardSystem] No playerTransform assigned and no GameObject with tag 'Player' found on {gameObject.name}.", this);
            }
        }
    }

    private void OnEnable()
    {
        if (!guardEnabled)
        {
            isGuardActive = false;
            currentGuardSide = GuardSide.None;
            return;
        }

        if (startWithGuardReady)
        {
            isGuardActive = true;
            timer = guardDuration;
            currentGuardSide = DetermineGuardSide();
        }
        else
        {
            isGuardActive = false;
            currentGuardSide = GuardSide.None;
            timer = guardCooldown;
        }
    }

    private void Update()
    {
        if (!guardEnabled)
        {
            isGuardActive = false;
            currentGuardSide = GuardSide.None;
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (isGuardActive)
            {
                isGuardActive = false;
                currentGuardSide = GuardSide.None;
                timer = guardCooldown;
            }
            else
            {
                isGuardActive = true;
                timer = guardDuration;
                currentGuardSide = DetermineGuardSide();
            }
        }
    }

    private GuardSide DetermineGuardSide()
    {
        if (playerTransform == null)
            return GuardSide.None;

        float playerX = playerTransform.position.x;
        float enemyX = transform.position.x;
        if (playerX < enemyX)
            return GuardSide.Left;
        else
            return GuardSide.Right;
    }

    /// <summary>
    /// Returns true if this guard blocks an attack from the given world position (X-axis only).
    /// </summary>
    public bool BlocksAttackFrom(Vector3 attackerPosition)
    {
        if (!IsGuardActive || CurrentGuardSide == GuardSide.None || !BlockAttackDashDamage)
            return false;

        float attackerX = attackerPosition.x;
        float enemyX = transform.position.x;
        if (CurrentGuardSide == GuardSide.Left && attackerX < enemyX)
            return true;
        if (CurrentGuardSide == GuardSide.Right && attackerX >= enemyX)
            return true;
        return false;
    }

    private void OnValidate()
    {
        guardDuration = Mathf.Max(0.1f, guardDuration);
        guardCooldown = Mathf.Max(0.1f, guardCooldown);
        counterDamageOnAttackDash = Mathf.Max(0f, counterDamageOnAttackDash);
    }
}
