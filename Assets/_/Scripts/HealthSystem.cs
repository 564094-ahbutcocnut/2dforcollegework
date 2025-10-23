using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class HealthSystem
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    // Property for MaxHealth (read-only)
    public int MaxHealth
    {
        get { return maxHealth; }
        private set { maxHealth = Mathf.Max(0, value); } // can't set below 0
    }

    // Property for CurrentHealth (read/write with rules)
    public int CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth); // keeps within 0–max
        }
    }


    public int GetHealth()
    {
        return currentHealth;
    }

    public HealthSystem(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        Debug.Log($"Took {amount} damage. Health now: {CurrentHealth}");
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        Debug.Log($"Healed {amount}. Health now: {CurrentHealth}");
    }

    public bool IsDead => CurrentHealth <= 0; // Expression-bodied property
}