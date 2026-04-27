using UnityEngine;

public class MenuNavigationButton : MonoBehaviour
{
    [Tooltip("Screen der beim Klick angezeigt wird.")]
    [SerializeField] private GameObject targetScreen;
    [Tooltip("Setzt Pause zurück und timeScale auf 1. Z.B. für 'Zurück ins Spiel'-Button.")]
    [SerializeField] private bool resumeOnNavigate = false;
    [Tooltip("Schließt das Overlay und setzt timeScale auf 1.")]
    [SerializeField] private bool closeOverlayOnNavigate = false;

    public void Navigate()
    {
        if (MenuManager.Instance == null)
        {
            Debug.LogWarning("[MenuNavigationButton] MenuManager nicht gefunden.");
            return;
        }

        if (resumeOnNavigate)
            MenuManager.Instance.Resume();
        else if (closeOverlayOnNavigate)
            MenuManager.Instance.CloseOverlay();
        else
            MenuManager.Instance.ShowScreen(targetScreen);
    }
}