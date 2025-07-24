using UnityEngine;
using System.Collections;

public class Map3_enemy2 : MonoBehaviour
{
    public LayerMask playerLayer;
    public Animator animator;
    public Rigidbody2D r2d;

    public float patrolSpeed = 2f;
    public float chaseSpeedMultiplier = 2f;
    private float currentSpeed;

    public Vector2 rayDirection;
    public float rayCastOffSet = 1f;
    public float detectionRange = 5f;

    public Transform pointA;
    public Transform pointB;
    private Vector3 currentTarget;

    public bool facingRight = true;
    private bool attacking = false;

    public int maxHP = 3;
    private int currentHP;
    private SpriteRenderer sr;
    private Color originalColor;

    private float stuckTimer = 0f;
    private float stuckDuration = 15f;

    private void Start()
    {
        currentTarget = pointA.position;
        currentSpeed = patrolSpeed;
        currentHP = maxHP;

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        InvokeRepeating(nameof(RaycastCheck), 0f, 0.1f);
    }

    private void Update()
    {
        if (!attacking)
        {
            Patrol();

            // Check if stuck for too long
            if (Vector3.Distance(transform.position, currentTarget) > 0.5f)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= stuckDuration)
                {
                    Die(); // Self-destruct
                }
            }
            else
            {
                stuckTimer = 0f; // Reset if reached target
            }
        }
        else
        {
            Chase();
        }
    }

    private void RaycastCheck()
    {
        Vector2 rayOrigin = (Vector2)transform.position + rayDirection.normalized * rayCastOffSet;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionRange, playerLayer);
        Debug.DrawRay(rayOrigin, rayDirection * detectionRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            attacking = true;
            animator.SetBool("attack", true);
        }
        else
        {
            StopChase();
        }
    }

    private void StopChase()
    {
        attacking = false;
        currentSpeed = patrolSpeed;
        animator.SetBool("attack", false);

        // Flip toward current patrol target after stopping chase
        if ((currentTarget.x < transform.position.x && facingRight) ||
            (currentTarget.x > transform.position.x && !facingRight))
        {
            Flip();
        }
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointA.position) ? pointB.position : pointA.position;
            Flip();
        }
    }

    private void Chase()
    {
        currentSpeed = patrolSpeed * chaseSpeedMultiplier;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 targetPos = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);

            if ((targetPos.x < transform.position.x && facingRight) ||
                (targetPos.x > transform.position.x && !facingRight))
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        // Rotate the sprite
        transform.Rotate(0f, 180f, 0f);

        // Flip the ray direction on X
        rayDirection.x *= -1;
    }


    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        StartCoroutine(FlashColor());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashColor()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    private void Die()
    {
        animator.SetTrigger("die");
        Destroy(gameObject, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }
}
