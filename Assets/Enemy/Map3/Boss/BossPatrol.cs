using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public GameObject bombPrefab;
    public float jumpCooldown = 3f;

    [Header("Spread Shot Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float spreadAngle = 60f;
    public float bulletSpeed = 7f;
    public float burstCooldown = 5f;
    public float timeBetweenBursts = 0.4f;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float hitColorDuration = 0.2f;
    public Color hitColor = Color.red;

    private float currentHealth;
    private Rigidbody2D rb;
    private bool movingToEnd = true;
    private bool facingRight = true;

    private float jumpTimer = 0f;
    private bool hasSpawnedBombThisJump = false;
    private bool isAttacking = false;
    private float burstTimer = 0f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isImmune = false;
    private bool isDead = false;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        jumpTimer = Random.Range(1f, jumpCooldown);
        burstTimer = Random.Range(2f, burstCooldown);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (!isAttacking)
        {
            Patrol();
        }

        DetectAndJump();
        HandleRandomJump();
        HandleBombDrop();

        burstTimer -= Time.fixedDeltaTime;
        if (burstTimer <= 0f && IsGrounded())
        {
            StartCoroutine(SpreadShotBurst());
            burstTimer = Random.Range(5f, burstCooldown + 5f);
        }
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
            Jump();
        }
    }

    void HandleRandomJump()
    {
        jumpTimer -= Time.fixedDeltaTime;

        if (jumpTimer <= 0f && IsGrounded())
        {
            Jump();
            jumpTimer = Random.Range(2f, jumpCooldown + 2f);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        hasSpawnedBombThisJump = false;
    }

    void HandleBombDrop()
    {
        if (!IsGrounded() && rb.linearVelocity.y < 0 && !hasSpawnedBombThisJump)
        {
            if (bombPrefab != null && groundCheck != null)
            {
                Instantiate(bombPrefab, groundCheck.position, Quaternion.identity);
                hasSpawnedBombThisJump = true;
            }
        }
        else if (IsGrounded())
        {
            hasSpawnedBombThisJump = false;
        }
    }

    IEnumerator SpreadShotBurst()
    {
        isAttacking = true;
        Vector3 originalScale = transform.localScale;

        int burstCount = Random.Range(2, 6);
        for (int b = 0; b < burstCount; b++)
        {
            FireSpread();
            yield return new WaitForSeconds(timeBetweenBursts);
        }

        transform.localScale = originalScale;
        isAttacking = false;
    }

    void FireSpread()
    {
        int bullets = 5;
        float angleStep = spreadAngle / (bullets - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < bullets; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * (facingRight ? Vector2.right : Vector2.left);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = dir * bulletSpeed;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isImmune) return;

        currentHealth -= damage;
        StartCoroutine(ShowHitColor());

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    IEnumerator ShowHitColor()
    {
        isImmune = true;
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitColorDuration);
        spriteRenderer.color = originalColor;
        isImmune = false;
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        Instantiate(bombPrefab, this.transform.position, Quaternion.identity);
        StartCoroutine(GoToNextScene());
        
    }

    IEnumerator GoToNextScene()
    {
        
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(4);
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




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }

    }
}
