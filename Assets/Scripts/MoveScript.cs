using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScript : MonoBehaviour
{

    private float horizontal;
    public int speed;
    public int jumpPower;
    private bool isFacingRight = true;

    private bool isWallSliding;
    public float wallSlidingSpeed;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.3f;
    private Vector2 wallJumpingPower = new Vector2(60f, 15f);

    private bool doubleJump;
    private bool wallDoubleJump = false;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public Transform respawnPoint;
    public Transform respawnPoint2;

    public SpriteRenderer flagBlue;
    public SpriteRenderer flagBlue2;
    private int flagNum = 1;

    public GameObject Player1WinScreen;
    public GameObject Player2WinScreen;

    public AudioSource jumpSoundEffect;
    public AudioSource checkpointSoundEffect;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        jumpSoundEffect = GameObject.FindGameObjectWithTag("JumpSound").GetComponent<AudioSource>();
        checkpointSoundEffect = GameObject.FindGameObjectWithTag("CheckpointSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); //returns -1, 0, or 1 depending on the direction were moving

        WallSlide(); //all functions to be run continously
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }

        if (IsGrounded()) //coyote time section
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W)) //jump buffer section
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (IsGrounded() && !Input.GetKey(KeyCode.W)) //double jump section
        {
            doubleJump = false;
        }
        if (IsWalled() && !Input.GetKey(KeyCode.W))
        {
            doubleJump = false;
        }

        if (Input.GetKeyDown(KeyCode.W)) //jump section
        {
            if ((coyoteTimeCounter > 0f || doubleJump == true) && jumpBufferCounter > 0f)
            {
                jumpSoundEffect.Play();
                rb.velocity = Vector2.up * jumpPower;
                doubleJump = !doubleJump;
                jumpBufferCounter = 0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Player1WinScreen.activeSelf && Player2WinScreen.activeSelf) //If both players win, restart the game
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x; //Jump in opposite direction of facing
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else if (wallDoubleJump && Input.GetKeyDown(KeyCode.W) && !IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = Vector2.up * jumpPower;
            wallDoubleJump = false;
        }
        else if (IsGrounded())
        {
            wallDoubleJump = false;
        }
        else 
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f; //to prevent the player from spamming jump multiple times
            wallDoubleJump = true;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip() //Flip the character whenever it turns
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Detect checkpoints
    {
        if (collision.tag == "Checkpoint 1")
        {
            checkpointSoundEffect.Play();
            Debug.Log("Player 1 touched Checkpoint 1");
            respawnPoint.position = transform.position;
            if (flagNum == 1)
            {
                flagBlue.color = Color.blue;
                flagNum = 2;
            }
            else
            {
                flagNum = 2;
            }
        }
        else if (collision.tag == "Checkpoint 2")
        {
            checkpointSoundEffect.Play();
            Debug.Log("Player 1 touched Checkpoint 2");
            respawnPoint.position = transform.position;
            if (flagNum == 2)
            {
                flagBlue2.color = Color.blue;
                flagNum = 3;
            }
            else 
            {
                flagNum = 3;
            }
        }
        if (collision.tag == "End")
        {
            Player1WinScreen.SetActive(true);
            Destroy(gameObject);
        }
    }
}
