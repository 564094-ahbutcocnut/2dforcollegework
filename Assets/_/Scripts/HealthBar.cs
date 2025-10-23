using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Set the slider max value (for new health systems)
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    // Update the slider when health changes
    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
