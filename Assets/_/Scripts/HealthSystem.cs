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
            // This is the clamp that prevents overhealing (keeps within 0–max)
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            //currentHealth = value;
        }
    }


    public int GetHealth()
    {
        return currentHealth;
    }

    public HealthSystem(int maxHealth, int currentHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        Debug.Log($"Took {amount} damage. Health now: {CurrentHealth}");
    }

    public void Heal(int amount)
    {
        // This setter runs the clamping logic defined above
        CurrentHealth += amount;
        // Keeping the log minimal here as Player.cs now handles detailed logging
        Debug.Log($"Attempted to heal {amount}. Health clamped by setter."); 
    }

    public bool IsDead => CurrentHealth <= 0; // Expression-bodied property
}