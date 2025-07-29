using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float JumpSpeed;
    public bool isGrounded;
    private bool facingRight = true;

    private int jumpCount = 0;
    public int maxJumps = 2;

    public SpriteRenderer sprite;
    public LayerMask groundLasyer;
    public LayerMask wallLayer;
    public Animator animator;
    public Rigidbody2D R2d;

    public Transform groundCheck;
    public Transform wallCheck;
    public float wallCheckDistance = 0.2f;

    private bool isTouchingWall;
    public bool isGrabbingWall;

    public Transform currentcheckpoint;
    public bool dead;

    // Crouch-related
    public CapsuleCollider2D capsuleCollider;
    public float normalHeight = 2f;
    public float crouchHeight = 1f;
    private Vector2 normalOffset;
    public Vector2 crouchOffset;

    private void Start()
    {
        R2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        currentcheckpoint = this.transform;
        normalOffset = capsuleCollider.offset;
    }

    private void Update()
    {
        movement();

        if (dead)
        {
            StartCoroutine(death());
        }
    }

    IEnumerator death()
    {
        animator.SetBool("death", true);
        yield return new WaitForSeconds(2.5f);
        dead = false;
        animator.SetBool("death", false);
        transform.position = currentcheckpoint.transform.position;
    }

    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Ground & wall checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLasyer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayer);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Wall grab logic
        if (isTouchingWall && !isGrounded && ((horizontalInput > 0 && facingRight) || (horizontalInput < 0 && !facingRight)))
        {
            isGrabbingWall = true;
        }
        else
        {
            isGrabbingWall = false;
        }

        // Crouch logic
        bool canCrouch = isGrounded && !isGrabbingWall && Mathf.Abs(R2d.linearVelocity.y) < 0.01f;
        bool isCrouching = verticalInput < 0 && canCrouch;

        if (isCrouching)
        {
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, crouchHeight);
            capsuleCollider.offset = crouchOffset;
            animator.SetBool("isCrouching", true);
        }
        else
        {
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, normalHeight);
            capsuleCollider.offset = normalOffset;
            animator.SetBool("isCrouching", false);
        }

        // Upward aiming logic
        bool isAimingUp = verticalInput > 0 && canCrouch;
        animator.SetBool("up", isAimingUp);

        // Wall grab effects
        if (isGrabbingWall)
        {
            R2d.gravityScale = 0;
            R2d.linearVelocity = Vector2.zero;
        }
        else
        {
            R2d.gravityScale = 1;

            // Stop horizontal movement while crouching or aiming up (on ground only)
            bool shouldFreeze = (isCrouching || isAimingUp);
            if (shouldFreeze)
            {
                R2d.linearVelocity = new Vector2(0, R2d.linearVelocity.y);
            }
            else
            {
                R2d.linearVelocity = new Vector2(horizontalInput * Speed, R2d.linearVelocity.y);
            }
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            R2d.linearVelocity = Vector2.up * JumpSpeed;
            jumpCount++;
        }

        // Animations
        animator.SetBool("wallGrab", isGrabbingWall);
        animator.SetBool("isJumping", !isGrounded && !isGrabbingWall);
        animator.SetFloat("Yvalo", R2d.linearVelocity.y);
        animator.SetFloat("Xvalo", Mathf.Abs(horizontalInput));

        // Flip
        if (horizontalInput > 0 && !facingRight)
            Flip();
        else if (horizontalInput < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.green;
            Vector3 direction = facingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + direction * wallCheckDistance);
        }
    }
}
