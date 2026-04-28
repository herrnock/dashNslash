using UnityEngine;
using System.Collections;

/// <summary>
/// Handles stun state for an enemy. When stunned, disables movement for a set duration.
/// </summary>
public class EnemyStunSystem : MonoBehaviour
{
    [SerializeField] private float stunDuration = 1.0f;
    public bool IsStunned { get; private set; } = false;

    private Coroutine stunCoroutine;

    /// <summary>
    /// Applies stun for the given duration. If already stunned, restarts the timer.
    /// </summary>
    public void ApplyStun(float duration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        stunCoroutine = StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        IsStunned = true;
        yield return new WaitForSeconds(duration);
        IsStunned = false;
        stunCoroutine = null;
    }
}
