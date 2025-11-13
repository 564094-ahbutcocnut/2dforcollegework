using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem; // 👈 This will now be visible
    public int contactDamage = 10;
    public float hitCooldown = 1f;
    public int startingHealth = 50;
    public int maxHealth = 100;
    [SerializeField] private HealthBar healthBar; // 👈 Reference to the UI health bar
    public GameManager gameManager; // Reference to the Game Manager script

    public string showStartMessage;

    public string showDeathMessage;

    public string nextSceneName;

    // Add this code to your player script
    public DisplayMessage messageDisplay;

    void Start()
    {
        // Create a new HealthSystem for the player
        healthSystem = new HealthSystem(maxHealth, startingHealth);
        healthBar.SetMaxHealth(maxHealth); // Set the bar's max health correctly

        // Show a message for 3 seconds
        messageDisplay.ShowMessage(showStartMessage);
        Invoke("ClearMessage", 3.0f);

        // Ensure GameManager is found
        GameObject gmObject = GameObject.Find("GameManager");
        if (gmObject != null)
        {
            gameManager = gmObject.GetComponent<GameManager>();
        }
    }

    void Update()
    {
        // CHECK FOR COIN BONUS (Continuous check)
        CheckCoinBonus();

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10); // Changed to use the public Heal method
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Heal(-10); // Changed to use the public Heal method
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

    // New method to handle the coin bonus
    void CheckCoinBonus()
    {
        // Check for null to prevent errors if GameManager isn't set up yet
        if (gameManager.coinsCounter >= 10)
        {

            Debug.Log($"[Coin Bonus] coins equals." + gameManager.coinsCounter);
            // Only apply the bonus if the player isn't already at max health
            if (healthSystem.GetHealth() < healthSystem.MaxHealth)
            {
                Debug.Log($"[Coin Bonus] Applying 50 HP heal. Coins will be spent.");

                // Call the public Heal method, which handles the health system and UI update
                Heal(50);

                // Reduce coins after successful heal
                gameManager.coinsCounter -= 10;
            }
            else
            {
                // This log helps debug why the heal was skipped (clamping/full health)
                Debug.Log($"[Coin Bonus] Skipped 50 HP heal: Player is already at Max Health ({healthSystem.MaxHealth}). Coins remain at 10.");
            }
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

        // The coin bonus logic has been removed from here.
    }


    public void TakeHit(int damage)
    {
        healthSystem.TakeDamage(damage);
        healthBar.SetHealth(healthSystem.GetHealth());


        if (healthSystem.IsDead)
        {
            Debug.Log("Player died!");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // Public method to handle healing and update the UI
    public void Heal(int amount)
    {
        int healthBefore = healthSystem.GetHealth();
        healthSystem.Heal(amount);
        int healthAfter = healthSystem.GetHealth();

        // Log the actual health gained for clarity
        int healthGained = healthAfter - healthBefore;
        if (healthGained < amount && healthBefore < healthSystem.MaxHealth)
        {
            Debug.Log($"Healed {healthGained} HP (was clamped by MaxHealth). Health now: {healthAfter}");
        }
        else
        {
            Debug.Log($"Healed {healthGained} HP. Health now: {healthAfter}");
        }

        healthBar.SetHealth(healthAfter);
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