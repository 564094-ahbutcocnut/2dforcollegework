using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem; // 👈 This will now be visible
    public int contactDamage = 10;
    public float hitCooldown = 1f;
    public int startingHealth = 100;
    [SerializeField] private HealthBar healthBar;  // 👈 Reference to the UI health bar

    public string showStartMessage;

    public string showDeathMessage;

    public string nextSceneName;

    // Add this code to your player script
    public DisplayMessage messageDisplay;

    void Start()
    {
        // Create a new HealthSystem for the player
        healthSystem = new HealthSystem(startingHealth);
        healthBar.SetMaxHealth(startingHealth);

        // Show a message for 3 seconds
        messageDisplay.ShowMessage(showStartMessage);
        Invoke("ClearMessage", 3.0f);

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

            messageDisplay.ShowMessage(showDeathMessage);

            // Reference the Player by its tag
            GameObject player = GameObject.FindWithTag("Player");
            // DReference the Player movement script
            var playerMovementScript = player.GetComponent<PlayerMovement>();

            // Disable the player's movement script
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = false;
                // This will disable the player's ability to move
            }

            Invoke("LoadNextLevel", 2f);

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


    // Function to hide the message 
    void ClearMessage()
    {
        messageDisplay.HideMessage();
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
