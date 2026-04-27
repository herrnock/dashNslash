using UnityEngine;

/// Detects left mouse clicks and sends world positions to MovementSystem.
/// Optionally shows a marker at the clicked position.
// Verarbeitet Spielerinput: Click-to-move und Moduswechsel
public class PlayerInputConnector : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem; // Reference to movement logic
    [SerializeField] private TargetMarkerView targetMarkerView; // Optional: shows marker at target
    [SerializeField] private CombatModeSystem combatModeSystem; // Optional: handles combat mode switching

    // Handles input for movement and combat mode switching
    private void Update()
    {
        // Handle combat mode switching
        if (combatModeSystem != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                combatModeSystem.SetAttackMode();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                combatModeSystem.SetDefenceMode();
            }
        }

        // On left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world XY
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            worldPosition.z = 0f;
            Vector2 target = new Vector2(worldPosition.x, worldPosition.y);
            // Send target to movement system
            movementSystem.SetTarget(target);
            // Show marker if assigned
            if (targetMarkerView != null)
            {
                targetMarkerView.ShowMarker(target);
            }
        }
    }
}
