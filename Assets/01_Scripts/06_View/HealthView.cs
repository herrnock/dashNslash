using UnityEngine;
using TMPro;

public class HealthView : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Update()
    {
        if (healthSystem == null) return;
        if (healthText == null) return;

        healthText.text = $"{healthSystem.CurrentHealth} / {healthSystem.MaxHealth}";
    }
}