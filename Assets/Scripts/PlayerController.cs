using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    #region Global Variables

    // public variables
    public float moveSpeed;
    public float jumpHeight;
    public int maxJumpCount;
    public int lives;
    public int maxLives;
    public bool allowMovement;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public List<GameObject> hitPoints;
    public CameraController mainCamera;

    // private variables
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool grounded;
    private int currJumpCount;
    private Vector3 originalPos;
    private Animator anim;
    private bool isMoving;
    private Vector2 lastMove;
    private float moveVelocity;

    // properties
    public bool isDead { get; private set; }

    #endregion

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        mainCamera = FindObjectOfType<CameraController>();

        lives = maxLives;
        currJumpCount = 0;
        originalPos = transform.position;
        isDead = false;

        if (hitPoints != null)
        {
            foreach (GameObject hitPoint in hitPoints)
            {
                Destroy(hitPoint);
            }
        }

        hitPoints = new List<GameObject>(lives);

        for (int i = 0; i < lives; i++)
        {
            GameObject hitPoint = new GameObject();
            hitPoint.AddComponent<SpriteRenderer>();
            SpriteRenderer hpRenderer = hitPoint.GetComponent<SpriteRenderer>();
            Sprite[] hpSprites = Resources.LoadAll<Sprite>("Art/hearts_sprites");
            hpRenderer.sprite = hpSprites[0];
            hpRenderer.sortingLayerName = "Foreground";
            hitPoint.transform.position = new Vector3(-1f + i, 6.3f, 1f);
            hitPoint.transform.SetParent(mainCamera.transform);

            hitPoints.Add(hitPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        mainCamera.xOffset = 0;

        // reset jump count when on ground
        if (grounded)
        {
            anim.SetBool("PlayerJumping", false);
            currJumpCount = 0;
        }
        else
        {
            anim.SetBool("PlayerJumping", true);
        }

        //look up
        if (Input.GetKeyDown(KeyCode.W) && allowMovement && grounded)
        {
            mainCamera.yOffset = 1;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            mainCamera.yOffset = 0;
        }

        //look down
        if (Input.GetKeyDown(KeyCode.S) && allowMovement && grounded)
        {
            mainCamera.yOffset = -1;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            mainCamera.yOffset = 0;
        }

        // jump controllers
        if (Input.GetKeyDown(KeyCode.Space) && allowMovement)
        {
            if (!grounded)
            {
                currJumpCount++;
            }

            lastMove = new Vector2(lastMove.x, Input.GetAxisRaw("Vertical"));

            // check if you can jump again in the air
            if (currJumpCount < maxJumpCount)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
        }

        moveVelocity = 0f;

        // move right
        if (Input.GetKey(KeyCode.D) && allowMovement)
        {
            //mainCamera.xOffset = 2;
            moveVelocity = moveSpeed;
            sr.flipX = false;
            MovePlayer();
        }

        // move left
        if (Input.GetKey(KeyCode.A) && allowMovement)
        {
            //mainCamera.xOffset = -2;
            moveVelocity = -moveSpeed;
            sr.flipX = true;
            MovePlayer();
        }

        rb.velocity = new Vector2(moveVelocity, rb.velocity.y);

        if (Input.GetKeyUp(KeyCode.R) && !allowMovement)
        {
            Restart();
        }

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetBool("PlayerMoving", isMoving);
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathTrap"))
        {
            transform.position = originalPos;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    public void MovePlayer()
    {
        isMoving = true;
        lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    /// <summary>
    /// Player enter's a door (leaves a level)
    /// </summary>
    public void EnterDoor()
    {
        allowMovement = false;
        anim.SetFloat("LastMoveY", 1f);
        anim.SetFloat("LastMoveX", 0f);
    }

    /// <summary>
    /// Restarts the player at the beginning of the level
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Takes away one hit point from the player
    /// </summary>
    public void LoseHitPoint()
    {
        if (lives > 0)
        {
            lives--;
            GameObject hitPoint = hitPoints[lives];
            SpriteRenderer hpRenderer = hitPoint.GetComponent<SpriteRenderer>();
            Sprite[] hpSprites = Resources.LoadAll<Sprite>("Art/hearts_sprites");
            hpRenderer.sprite = hpSprites[2];
            hpRenderer.sortingLayerName = "Foreground";
        }
    }

    /// <summary>
    /// Adds a hit point to the player
    /// </summary>
    public void RestoreHitPoint()
    {
        if (lives < maxLives)
        {
            GameObject hitPoint = hitPoints[lives + 1];
            SpriteRenderer hpRenderer = hitPoint.GetComponent<SpriteRenderer>();
            Sprite[] hpSprites = Resources.LoadAll<Sprite>("Art/hearts_sprites");
            hpRenderer.sprite = hpSprites[0];
            hpRenderer.sortingLayerName = "Foreground";
        }
    }

    /// <summary>
    /// Sets if the player should be frozen in place
    /// </summary>
    /// <param name="frozen">True if the player should be frozen</param>
    public void FreezePlayer(bool frozen)
    {
        if (!frozen)
        {
            allowMovement = true;
            rb.gravityScale = 4;
        }
        else
        {
            allowMovement = false;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    /// <summary>
    /// Sets if the player sprite should be displayed
    /// </summary>
    /// <param name="show">True if should be displayed</param>
    public void ShowPlayer(bool show)
    {
        sr.enabled = show;
    }

    /// <summary>
    /// Set player state to dead
    /// </summary>
    public void SetDead(bool isDead)
    {
        if (isDead)
        {
            isDead = true;
            allowMovement = false;
            anim.SetBool("PlayerDead", true);
        }
        else
        {
            isDead = false;
            allowMovement = true;
            anim.SetBool("PlayerDead", false);
        }
    }

    /// <summary>
    /// Plays the player's hurt sound effect
    /// </summary>
    public void PlayHurtSound()
    {
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Knockback the player (used for after hit by enemy)
    /// </summary>
    public void Knockback()
    {
        //rb.velocity = new Vector2(-250f, 10f);
    }
}
