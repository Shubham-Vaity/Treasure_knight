using System.Collections;
using UnityEngine;

public class enemy2Map2 : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    public bool bulletFired = true;

    public Animator animator;
    public GameObject explosion;
    public GameObject firespot;
    public GameObject bullet;

    public float detectionRange = 5f;
    public float rayCastOffSet = 1f;
    public LayerMask playerLayer;
    public Vector2 rayDirection;

    private Vector3 nextPoint;
    private bool facingRight = true;

    void Start()
    {
        nextPoint = pointB.position;
        InvokeRepeating(nameof(RaycastCheck), 0f, 0.1f);
    }

    void Update()
    {
        MoveEnemy();
        FlipBasedOnDirection();
    }

    void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);

        if (transform.position == nextPoint)
        {
            nextPoint = (nextPoint == pointA.position) ? pointB.position : pointA.position;
        }
    }

    void FlipBasedOnDirection()
    {
        float direction = nextPoint.x - transform.position.x;

        if (direction > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Flip the X axis
        transform.localScale = localScale;
    }

    void RaycastCheck()
    {
        Vector2 rayOrigin = (Vector2)transform.position + rayDirection.normalized * rayCastOffSet;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionRange, playerLayer);

        Debug.DrawRay(rayOrigin, rayDirection * detectionRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (bulletFired)
            {
                animator.SetBool("Attacking", true);
                Instantiate(bullet, firespot.transform.position, firespot.transform.rotation);
                bulletFired = false;
                StartCoroutine(AttackDelay());
            }
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.1f);
        bulletFired = true;
    }

    void death()
    {
        animator.SetBool("Dead", true);
        StartCoroutine(DeadlyDelay(0.1f));
    }

    IEnumerator DeadlyDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            death();
        }
    }
}
