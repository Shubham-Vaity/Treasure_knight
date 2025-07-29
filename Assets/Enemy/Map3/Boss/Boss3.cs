using UnityEngine;

public class BossPatrol : MonoBehaviour
{
    public float patrolStartX;
    public float patrolEndX;
    public float moveSpeed = 3f;
    public float reachThreshold = 0.1f;
    public float jumpForce = 7f;
    public float rayLength = 3f;
    public LayerMask playerLayer;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public GameObject bombPrefab;           // Bomb prefab to spawn
    public float jumpCooldown = 3f;         // Time interval between random jumps

    private Rigidbody2D rb;
    private bool movingToEnd = true;
    private bool facingRight = true;

    private float jumpTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = Random.Range(1f, jumpCooldown); // Randomize first jump
    }

    void FixedUpdate()
    {
        Patrol();
        DetectAndJump();
        HandleRandomJump();
    }

    void Patrol()
    {
        float targetX = movingToEnd ? patrolEndX : patrolStartX;
        float direction = Mathf.Sign(targetX - transform.position.x);

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if ((movingToEnd && transform.position.x >= patrolEndX) ||
            (!movingToEnd && transform.position.x <= patrolStartX))
        {
            movingToEnd = !movingToEnd;
        }

        if (direction > 0 && !facingRight)
            Flip();
        else if (direction < 0 && facingRight)
            Flip();
    }

    void DetectAndJump()
    {
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;
        Vector2 rayOrigin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, playerLayer);

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player") && IsGrounded())
        {
            JumpAndSpawnBomb();
        }
    }

    void HandleRandomJump()
    {
        jumpTimer -= Time.fixedDeltaTime;

        if (jumpTimer <= 0f && IsGrounded())
        {
            JumpAndSpawnBomb();
            jumpTimer = Random.Range(2f, jumpCooldown + 2f); // Randomize next jump
        }
    }

    void JumpAndSpawnBomb()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (bombPrefab != null && groundCheck != null)
        {
            Instantiate(bombPrefab, groundCheck.position, Quaternion.identity);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
