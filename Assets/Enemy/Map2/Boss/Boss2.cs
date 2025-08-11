using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Boss2 : MonoBehaviour
{
    [Header("References")]
    public GameObject firespot;
    public GameObject bullet1;
    public GameObject bombPrefab;
    public Animator animator;

    [Header("Settings")]
    public float floatSpeed = 2f;
    public int HP = 10;
    public float attackInterval = 4f;
    public Vector3 bombDropOffset = new Vector3(0f, -1f, 0f);

    [Header("State")]
    private bool isDead = false;
    private bool isAttacking = false;
    private bool facingRight = true;
    private bool immunityFrames = false;

    [Header("Patrol")]
    public Vector3 leftLimit = new Vector3(140f, 15f, 0f);
    public Vector3 rightLimit = new Vector3(170f, 15f, 0f);
    private bool movingRight = true;

    private GameObject player;
    public GameObject explosion;
    public GameObject bigexplosion;
    private SpriteRenderer sr;
    private float nextBombTime;

    public AudioClip sound;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        CheckInitialDirection();
        ScheduleNextBomb();

        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        if (!isDead && !isAttacking)
        {
            Patrol();
            HandleBombDropping();
        }

        AimAtPlayer();
    }

    void CheckInitialDirection()
    {
        float distToLeft = Vector3.Distance(transform.position, leftLimit);
        float distToRight = Vector3.Distance(transform.position, rightLimit);
        movingRight = distToRight < distToLeft;

        bool shouldFaceRight = movingRight;

        if ((shouldFaceRight && !facingRight) || (!shouldFaceRight && facingRight))
        {
            Flip();
        }
    }

    void Patrol()
    {
        float step = floatSpeed * Time.deltaTime;

        if (movingRight)
        {
            transform.position += new Vector3(step, 0, 0);
            if (transform.position.x >= rightLimit.x)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.position -= new Vector3(step, 0, 0);
            if (transform.position.x <= leftLimit.x)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void HandleBombDropping()
    {
        if (Time.time >= nextBombTime && bombPrefab != null)
        {
            Vector3 spawnPos = transform.position + bombDropOffset;
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            ScheduleNextBomb();
        }
    }

    void ScheduleNextBomb()
    {
        nextBombTime = Time.time + Random.Range(1f, 3f);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void AimAtPlayer()
    {
        if (player == null || firespot == null) return;

        Vector3 direction = player.transform.position - firespot.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Flip the angle if the fire is facing the wrong direction
        firespot.transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
    }


    IEnumerator AttackLoop()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(attackInterval);
            if (!isDead)
            {
                yield return StartCoroutine(FlamethrowerAttack());
            }
        }
    }

    IEnumerator FlamethrowerAttack()
    {
        isAttacking = true;
      

        float timer = 0f;
        float duration = 2.5f;

        while (timer < duration)
        {
            Instantiate(bullet1, firespot.transform.position, firespot.transform.rotation);
            yield return new WaitForSeconds(0.03f);
            timer += 0.1f;
        }

       
        isAttacking = false;
    }

    void TakeDamage()
    {
        if (!immunityFrames)
        {
            
            StartCoroutine(FlashDamageColor());
            immunityFrames = true;
            AudioSource.PlayClipAtPoint(sound, transform.position);
            HP--;

            if (HP <= 0 && !isDead)
            {
                StartCoroutine(Death());
            }

            StartCoroutine(DamageCooldown(0.5f));
        }
    }

    IEnumerator FlashDamageColor()
    {
        
        animator.SetBool("hit", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("hit", false);
        
    }

    IEnumerator DamageCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        immunityFrames = false;
    }

    IEnumerator Death()
    {
        isDead = true;
        animator.SetBool("dead", true);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(explosion, firespot.transform.position, firespot.transform.rotation);
        }
        StartCoroutine(NextStage(2f));
        yield return new WaitForSeconds(0.5f);
        transform.localScale = transform.localScale * 3;
        animator.SetBool("dead", false);
        Instantiate(bigexplosion, firespot.transform.position, firespot.transform.rotation);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();
        }

        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                Vector2 knockback = new Vector2(dir.x * 40f, 20f);
                playerRb.linearVelocity = Vector2.zero;
                playerRb.AddForce(knockback, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator NextStage(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(3);
    }

}
