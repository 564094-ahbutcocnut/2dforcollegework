using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem; // 👈 This will now be visible
    public int contactDamage = 10;
    public float hitCooldown = 1f;
    public int startingHealth = 100;
    [SerializeField] private HealthBar healthBar;  // 👈 Reference to the UI health bar



    void Start()
    {
        // Create a new HealthSystem for the player
        healthSystem = new HealthSystem(startingHealth);
        healthBar.SetMaxHealth(startingHealth);

    }

    void Update()
    {
        // Press keys to test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            healthSystem.TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthSystem.Heal(10);
        }

        if (healthSystem.IsDead == true)
        {
            Debug.Log("Player is dead!");
        }
    }

    // Detect when Player hits another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we hit an enemy
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("Player hit Enemy!");
            enemy.TakeHit(contactDamage);
        }
    }
    
    public void TakeHit(int damage)
    {
        healthSystem.TakeDamage(damage);
        healthBar.SetHealth(healthSystem.GetHealth());


        if (healthSystem.IsDead)
        {
            Debug.Log("Player died!");
        }
    }

    public void Heal(int amount)
    {
        healthSystem.Heal(amount);
        healthBar.SetHealth(healthSystem.GetHealth());


    }
}
