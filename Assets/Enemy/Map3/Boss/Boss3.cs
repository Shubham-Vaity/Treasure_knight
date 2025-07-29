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

    private Rigidbody2D rb;
    private bool movingToEnd = true;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Patrol();
        DetectAndJump();
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
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red); // Visible ray in Scene view

        if (hit.collider != null && hit.collider.CompareTag("Player") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
