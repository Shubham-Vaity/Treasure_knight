using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossPatrol : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float moveSpeed = 3f;
    public float reachThreshold = 0.2f;
    public float jumpForce = 10f;
    public LayerMask playerLayer;
    public float detectionRange = 3f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool movingToEnd = true;
    private bool facingRight = true;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogWarning("Player not found. Make sure the Player has the 'Player' tag.");
    }

    void Update()
    {
        Patrol();
        DetectPlayerAndJump();
    }

    void Patrol()
    {
        Vector3 target = movingToEnd ? endPoint : startPoint;
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        rb.MovePosition(nextPosition);

        if (target.x > transform.position.x && !facingRight)
            Flip();
        else if (target.x < transform.position.x && facingRight)
            Flip();

        if (Vector3.Distance(transform.position, target) < reachThreshold)
            movingToEnd = !movingToEnd;
    }

    void DetectPlayerAndJump()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;

        Debug.DrawRay(rayOrigin, rayDirection * detectionRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionRange, playerLayer);
        if (hit.collider != null && IsGrounded())
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
