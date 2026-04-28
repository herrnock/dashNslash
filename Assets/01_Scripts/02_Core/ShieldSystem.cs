using UnityEngine;
using System.Collections;

/// <summary>
/// Manages shield charges for the player. Handles consumption, full recharge, and optional auto-recharge.
/// Does not handle collision, UI, or mode switching.
/// </summary>
public class ShieldSystem : MonoBehaviour
{
    [Header("Shield Settings")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges = 3;
    [SerializeField] private bool autoRecharge = false;
    [SerializeField] private float rechargeDelay = 2f;
    [SerializeField] private bool rechargeOnModeSwitch = false;
    public bool RechargeOnModeSwitchEnabled => rechargeOnModeSwitch;
    /// <summary>
    /// Recharges shield to full if rechargeOnModeSwitch is enabled.
    /// </summary>
    public void RechargeOnModeSwitch()
    {
        if (rechargeOnModeSwitch)
        {
            RechargeFull();
        }
    }

    private Coroutine rechargeCoroutine;

    // Public read-only properties
    public int MaxCharges => maxCharges;
    public int CurrentCharges => currentCharges;
    public bool HasCharges => currentCharges > 0;
    public bool IsBroken => currentCharges == 0;

    private void Awake()
    {
        // Always initialize currentCharges to maxCharges on start
        currentCharges = maxCharges;
    }

    /// <summary>
    /// Attempts to consume a shield charge. Returns true if successful, false if shield is empty.
    /// </summary>
    public bool ConsumeCharge()
    {
        if (currentCharges > 0)
        {
            currentCharges--;
            if (autoRecharge)
            {
                StartRechargeTimer();
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Fully restores shield charges to max.
    /// </summary>
    public void RechargeFull()
    {
        currentCharges = maxCharges;
        if (rechargeCoroutine != null)
        {
            StopCoroutine(rechargeCoroutine);
            rechargeCoroutine = null;
        }
    }

    /// <summary>
    /// Starts the recharge timer if not already running.
    /// </summary>
    public void StartRechargeTimer()
    {
        if (rechargeCoroutine != null)
            return;
        rechargeCoroutine = StartCoroutine(RechargeAfterDelay());
    }

    private IEnumerator RechargeAfterDelay()
    {
        yield return new WaitForSeconds(rechargeDelay);
        RechargeFull();
        rechargeCoroutine = null;
    }
}
