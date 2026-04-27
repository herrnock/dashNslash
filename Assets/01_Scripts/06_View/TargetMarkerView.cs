using UnityEngine;

// Zeigt den Zielmarker für Bewegung an (nur visuell)

public class TargetMarkerView : MonoBehaviour
{
    [SerializeField] private GameObject markerObject; // The marker GameObject to show

    // Setzt Marker auf XY-Position und zeigt ihn an
    public void ShowMarker(Vector2 position)
    {
        if (markerObject == null)
            return;
        // Move marker to XY position and activate
        markerObject.transform.position = new Vector3(position.x, position.y, 0f);
        markerObject.SetActive(true);
    }
}
