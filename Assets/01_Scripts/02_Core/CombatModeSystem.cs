using UnityEngine;

// Speichert und verwaltet den aktuellen Kampfmodus (Attack/Defence)
public class CombatModeSystem : MonoBehaviour
{
    // Enum für den aktuellen Kampfmodus
    public enum CombatMode
    {
        Attack,
        Defence
    }

    private CombatMode currentMode = CombatMode.Attack; // Default mode is Attack

    // Gibt den aktuellen Modus zurück (nur lesbar)
    public CombatMode Mode => currentMode;

    // Setzt den Modus auf Attack
    public void SetAttackMode()
    {
        currentMode = CombatMode.Attack;
    }

    // Setzt den Modus auf Defence
    public void SetDefenceMode()
    {
        currentMode = CombatMode.Defence;
    }
}
