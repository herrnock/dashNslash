using UnityEngine;

public class EnemyGuardView : MonoBehaviour
{
    [SerializeField] private EnemyGuardSystem guardSystem;
    [SerializeField] private GameObject leftShieldVisual;
    [SerializeField] private GameObject rightShieldVisual;
    [SerializeField] private bool hideVisualsOnStart = true;

    private bool leftWarningLogged = false;
    private bool rightWarningLogged = false;

    private void Awake()
    {
        if (guardSystem == null)
        {
            guardSystem = GetComponent<EnemyGuardSystem>();
        }
        if (guardSystem == null)
        {
            Debug.LogWarning($"[EnemyGuardView] No EnemyGuardSystem found on {gameObject.name}. Disabling EnemyGuardView.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        if (hideVisualsOnStart)
        {
            if (leftShieldVisual != null)
                leftShieldVisual.SetActive(false);
            if (rightShieldVisual != null)
                rightShieldVisual.SetActive(false);
        }
        if (leftShieldVisual == null && !leftWarningLogged)
        {
            Debug.LogWarning($"[EnemyGuardView] Left shield visual not assigned on {gameObject.name}.", this);
            leftWarningLogged = true;
        }
        if (rightShieldVisual == null && !rightWarningLogged)
        {
            Debug.LogWarning($"[EnemyGuardView] Right shield visual not assigned on {gameObject.name}.", this);
            rightWarningLogged = true;
        }
    }

    private void LateUpdate()
    {
        if (guardSystem == null)
            return;

        // Hide both if inactive or GuardSide.None
        if (!guardSystem.IsGuardActive || guardSystem.CurrentGuardSide == EnemyGuardSystem.GuardSide.None)
        {
            if (leftShieldVisual != null) leftShieldVisual.SetActive(false);
            if (rightShieldVisual != null) rightShieldVisual.SetActive(false);
            return;
        }

        if (guardSystem.CurrentGuardSide == EnemyGuardSystem.GuardSide.Left)
        {
            if (leftShieldVisual != null) leftShieldVisual.SetActive(true);
            if (rightShieldVisual != null) rightShieldVisual.SetActive(false);
        }
        else if (guardSystem.CurrentGuardSide == EnemyGuardSystem.GuardSide.Right)
        {
            if (leftShieldVisual != null) leftShieldVisual.SetActive(false);
            if (rightShieldVisual != null) rightShieldVisual.SetActive(true);
        }
    }
}
