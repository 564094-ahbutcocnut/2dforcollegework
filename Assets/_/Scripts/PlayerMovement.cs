using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private float wallJumpCooldown;
    public GameManager gameManager; // Reference to the Game Manager script
    public bool deathState = false; // Set default death state to false
    private float horizontalInput;


    private SpriteRenderer spritegraphic;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        spritegraphic = GetComponentInChildren<SpriteRenderer>();
    }

    public void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // 1. Handle Cooldown and Horizontal Movement
        if (wallJumpCooldown > 0)
        {
            // Decrement cooldown (only for movement blocking)
            wallJumpCooldown -= Time.deltaTime;
        }

        // Only allow horizontal movement if not in a wall jump cooldown (or if the cooldown is just for a short 'launch' period)
        if (wallJumpCooldown <= 0)
        {
            // Standard horizontal movement
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }

        // 2. Player Flip Logic
        // Keep your existing flip logic (it's correct)
        if (horizontalInput > 0.01f)
            spritegraphic.flipX = false;
        else if (horizontalInput < -0.01f)
            spritegraphic.flipX = true;

        // 3. Animator Updates
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // 4. Wall Slide Logic (runs regardless of movement cooldown)
        if (onWall() && !isGrounded() && wallJumpCooldown <= 0)
        {
            // Apply wall slide effect
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero; // Or a controlled vertical slide velocity
        }
        else
        {
            // Restore normal gravity when not wall sliding
            body.gravityScale = 2;
        }

        // 5. Jump Input (Always check for jump)
        if (Input.GetKeyDown(KeyCode.Space)) // Using GetKeyDown for cleaner single-press jump
        {
            Jump();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            gameManager.coinsCounter += 1;
            Destroy(other.gameObject);
            Debug.Log("Player has collected a coin!");
        }
       
        if (other.gameObject.tag == "Finish")
        {
            // Game will reload in 3 seconds
            gameManager.Invoke("ReloadLevel", 3);
        }
        if (other.gameObject.tag == "Finish")
        {
            // Game will reload in 3 seconds
            gameManager.Invoke("EndGame", 3);
        }

    }



    // Inside PlayerMovement.cs
    public void Jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            int wallDir = getWallDirection();

            if (wallDir != 0) // Wall detected
            {
                float wallJumpX = 7.5f;
                float wallJumpY = 7.5f;
                float wallJumpDuration = 0.3f;

                // Apply force AWAY from the wall (jumpDirection is the opposite of the wall's direction)
                float jumpDirection = -wallDir;

                body.linearVelocity = new Vector2(jumpDirection * wallJumpX, wallJumpY);

                // Face the direction of the jump
                spritegraphic.flipX = jumpDirection < 0; // Flip sprite if jumping left

                // ... (keep the rest of your wall jump logic)
                body.gravityScale = 7;
                wallJumpCooldown = wallJumpDuration;
                anim.SetTrigger("jump");
            }
        }
    }

    // Inside PlayerMovement.cs
    public bool onWall()
    {
        // Check if the player is actively trying to move left or right
        // Use the sign of the horizontal input to determine the raycast direction
        float direction = horizontalInput != 0 ? Mathf.Sign(horizontalInput) : Mathf.Sign(transform.localScale.x);

        // If the player is not pressing a direction, we can use transform.localScale.x 
        // to check for the wall they are currently stuck to. However, to simplify the wall jump check
        // we should make sure we are only looking for a wall when we are pressing *against* it.

        // Let's use the player's facing direction if they aren't moving, 
        // but the actual input direction is usually best for "sticking" to a wall.

        // OPTION A: Using a fixed size check (more robust for just detecting contact)
        // We check both left and right simultaneously
        RaycastHit2D hitRight = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.right, 0.1f, wallLayer);
        RaycastHit2D hitLeft = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.left, 0.1f, wallLayer);

        return hitRight.collider != null || hitLeft.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Inside PlayerMovement.cs (Add this helper method)
    private int getWallDirection()
    {
        // Check for a wall to the right (direction +1)
        RaycastHit2D hitRight = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.right, 0.1f, wallLayer);
        if (hitRight.collider != null)
        {
            return 1;
        }

        // Check for a wall to the left (direction -1)
        RaycastHit2D hitLeft = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.left, 0.1f, wallLayer);
        if (hitLeft.collider != null)
        {
            return -1;
        }

        return 0; // No wall detected
    }


}
