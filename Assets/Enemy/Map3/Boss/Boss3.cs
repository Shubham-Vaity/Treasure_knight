using UnityEngine;
using System.Collections;

public class BossType3 : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed = 3f;
    private bool movingRight = true;

    public Transform fireSpot;
    public GameObject flamePrefab;
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform bombDropPoint;
    public Transform player;
    public Transform groundCheck;

    public LayerMask playerLayer;
    public float raycastDistance = 5f;
    public float jumpForce = 7f;
    public Rigidbody2D rb;
    public Animator anim;

    private bool isAttacking = false;
    private bool isGrounded;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(RandomAttackRoutine());
    }

    private void Update()
    {
        if (!isAttacking)
            Patrol();

        CheckPlayerRay();
    }

    void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.position.x >= rightPoint.position.x)
                Flip(false);
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x <= leftPoint.position.x)
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

    void AimFireSpotAtPlayer()
    {
        if (player == null || fireSpot == null) return;
        Vector3 dir = player.position - fireSpot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        fireSpot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    IEnumerator RandomAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));

            if (!isAttacking)
            {
                int attack = Random.Range(0, 2); // 0 = flame, 1 = spread burst
                isAttacking = true;
                AimFireSpotAtPlayer();

                if (attack == 0)
                    StartCoroutine(FlameThrowerAttack());
                else
                    StartCoroutine(SpreadShotBurst());
            }
        }
    }

    IEnumerator FlameThrowerAttack()
    {
        anim.SetTrigger("Flame");
        Instantiate(flamePrefab, fireSpot.position, fireSpot.rotation);
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    IEnumerator SpreadShotBurst()
    {
        anim.SetTrigger("Shoot");
        int burstCount = Random.Range(3, 6);
        for (int i = 0; i < burstCount; i++)
        {
            FireSpreadBullets();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    void FireSpreadBullets()
    {
        float[] angles = { -15f, 0f, 15f };
        foreach (float a in angles)
        {
            Quaternion rot = fireSpot.rotation * Quaternion.Euler(0, 0, a);
            Instantiate(bulletPrefab, fireSpot.position, rot);
        }
    }

    void CheckPlayerRay()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, playerLayer);
        if (hit.collider != null && IsGrounded() && !isAttacking)
        {
            StartCoroutine(JumpAttack());
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
    }

    IEnumerator JumpAttack()
    {
        isAttacking = true;

        // Drop bomb
        Instantiate(bombPrefab, bombDropPoint.position, Quaternion.identity);

        // Jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }
}
