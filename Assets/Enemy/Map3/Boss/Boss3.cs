using UnityEngine;

public class BossType3 : MonoBehaviour
{
    [Header("Movement")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 3f;
    private bool movingRight = true;

    [Header("Attack Settings")]
    public GameObject bulletPrefab;
    public GameObject flamePrefab;
    public GameObject bombPrefab;
    public Transform firePoint;
    public Transform bombDropPoint;
    public float attackInterval = 3f;

    [Header("Jump Attack")]
    public float rayDistance = 5f;
    public LayerMask playerLayer;
    public float jumpForce = 10f;
    public Rigidbody2D rb;

    private GameObject player;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private bool isJumping = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isAttacking || isJumping) return;

        Patrol();
        DetectPlayerWithRaycast();

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            StartRandomAttack();
        }
    }

    void Patrol()
    {
        float direction = movingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (movingRight && transform.position.x >= rightPoint.position.x)
        {
            Flip(false);
        }
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
        {
            Flip(true);
        }
    }

    void Flip(bool faceRight)
    {
        movingRight = faceRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceRight ? 1 : -1);
        transform.localScale = scale;
    }

    void StartRandomAttack()
    {
        isAttacking = true;
        int attackType = Random.Range(0, 3); // 0, 1, 2

        switch (attackType)
        {
            case 0: Invoke(nameof(SingleShot), 0.5f); break;
            case 1: StartCoroutine(BurstShot()); break;
            case 2: StartCoroutine(FlameThrower()); break;
        }
    }

    void SingleShot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        isAttacking = false;
    }

    System.Collections.IEnumerator BurstShot()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            yield return new WaitForSeconds(0.3f);
        }
        isAttacking = false;
    }

    System.Collections.IEnumerator FlameThrower()
    {
        GameObject flame = Instantiate(flamePrefab, firePoint.position, firePoint.rotation);
        yield return new WaitForSeconds(2f);
        Destroy(flame);
        isAttacking = false;
    }

    void DetectPlayerWithRaycast()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, rayDistance, playerLayer);

        Debug.DrawRay(firePoint.position, direction * rayDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            StartCoroutine(JumpAndDropBomb());
        }
    }

    System.Collections.IEnumerator JumpAndDropBomb()
    {
        isJumping = true;
        isAttacking = true;

        Vector3 preAttackScale = transform.localScale;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        yield return new WaitForSeconds(0.5f);

        Instantiate(bombPrefab, bombDropPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(1.5f); // Wait to land

        // Restore direction and state
        transform.localScale = preAttackScale;
        isJumping = false;
        isAttacking = false;
    }
}
