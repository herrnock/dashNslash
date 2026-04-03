using UnityEngine;

public class MenuNavigationButton : MonoBehaviour
{
    [Tooltip("Name des Screens der angezeigt werden soll.")]
    [SerializeField] private string screenID;

    public void Navigate()
    {
        if (MenuManager.Instance == null)
        {
            Debug.LogWarning("[MenuNavigationButton] MenuManager nicht gefunden.");
            return;
        }

        MenuManager.Instance.ShowScreen(screenID);
    }
}