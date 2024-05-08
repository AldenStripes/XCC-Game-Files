using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScript : MonoBehaviour //This script doesnt just contain move elements because I used the same script for a bunch of things including checkpoints
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

    private bool zeroVelocity = false;

    public MainMenu mainMenu;

    // SerializeField allows Unity to see the component even though it is private
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private Animator animRun;
    private Animator animJump;

    // Start is called before the first frame update
    void Start()
    {
        animRun = GetComponent<Animator>();
        animJump = GetComponent<Animator>();
        jumpSoundEffect = GameObject.FindGameObjectWithTag("JumpSound").GetComponent<AudioSource>();
        checkpointSoundEffect = GameObject.FindGameObjectWithTag("CheckpointSound").GetComponent<AudioSource>();
        //mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>
    }

    // Update is called once per frame
    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal"); //returns -1, 0, or 1 depending on the direction were moving

        if (horizontal == 0) { // if player not moving
            animRun.SetBool("isRunning", false);
        }   else {
            animRun.SetBool("isRunning", true);
        }

        WallSlide(); //all functions to be run continously
        WallJump();

        if (!isWallJumping) {
            Flip();
        }

        // Allows player to jump 0.2s after leaving the ground to give some leniency (called coyote time for some reason)
        if (IsGrounded()) //coyote time section
        {
            coyoteTimeCounter = coyoteTime;
            animJump.SetBool("isJumping", false);
        }
        else {
            coyoteTimeCounter -= Time.deltaTime;
            animJump.SetBool("isJumping", true);
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
        if (IsWalled() && !Input.GetKey(KeyCode.W)) {
            doubleJump = false;
        }
        // if (!IsGrounded()) { // so when you walk off a ledge you can still double jump
        //     doubleJump = true;
        // }
        if (Input.GetKeyDown(KeyCode.W)) //if player presses jump
        {
            if ((coyoteTimeCounter>0f && !doubleJump) && jumpBufferCounter > 0f) {
                jumpSoundEffect.Play(); // play jump sound affect
                rb.velocity = Vector2.up * jumpPower; // move velocity of player upward
                doubleJump = !doubleJump; // set double jump-able to false
                jumpBufferCounter = 0f; 
            }
            if (!IsGrounded() && doubleJump && jumpBufferCounter > 0f) {
                rb.velocity = Vector2.up * jumpPower; // jump
                jumpSoundEffect.Play();
                doubleJump = false;
                jumpBufferCounter = 0f;
            }
        }

        // The longer you hold the jump button, the higher you go (up to a point)
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Player1WinScreen.activeSelf && Player2WinScreen.activeSelf) //If both players win, restart the game
        {
            //LoadNextLevel();
        }

        if (zeroVelocity) {
            rb.velocity = new Vector2(0, 0);
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
        else if (collision.tag == "Checkpoint 2") {
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
            zeroVelocity = true;
        }
    }
}
