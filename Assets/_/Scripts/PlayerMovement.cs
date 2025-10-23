using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;
    private float wallJumpCooldown;
    public GameManager gameManager; // Reference to the Game Manager script
    public bool deathState = false; // Set default death state to false


    private SpriteRenderer spritegraphic;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        spritegraphic = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            spritegraphic.flipX = false;

            //transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            spritegraphic.flipX = true;
            //transform.localScale = new Vector3(-1, 1, 1);


        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Wall Jump logic
        if(wallJumpCooldown < 0.2f)
        {

            if (Input.GetKey(KeyCode.Space) && isGrounded())
                Jump();

            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if(onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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



    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        anim.SetTrigger("jump");             
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
            return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

}
