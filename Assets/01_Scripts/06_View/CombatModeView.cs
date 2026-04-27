using UnityEngine;

// Zeigt den aktuellen Kampfmodus farblich am Spieler an
public class CombatModeView : MonoBehaviour
{
    [SerializeField] private CombatModeSystem combatModeSystem; // Referenz auf CombatModeSystem
    [SerializeField] private SpriteRenderer spriteRenderer;      // Referenz auf den SpriteRenderer des Spielers

    // Farben für die Modi
    [SerializeField] private Color attackColor = new Color(1f, 0.4f, 0f); // Orange/Rot
    [SerializeField] private Color defenceColor = Color.blue;

    private void Update()
    {
        if (combatModeSystem == null || spriteRenderer == null)
            return;

        // Setze Farbe je nach Modus
        if (combatModeSystem.Mode == CombatModeSystem.CombatMode.Attack)
        {
            spriteRenderer.color = attackColor;
        }
        else if (combatModeSystem.Mode == CombatModeSystem.CombatMode.Defence)
        {
            spriteRenderer.color = defenceColor;
        }
    }
}
