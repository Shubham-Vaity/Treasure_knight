using System.Collections;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("References")]
    public GameObject firespot;
    public GameObject bullet1;
    public Animator animator;

    [Header("Settings")]
    public float floatSpeed = 2f;
    public int HP = 10;
    public float attackInterval = 4f;

    [Header("State")]
    private bool isDead = false;
    private bool isAttacking = false;
    private bool facingRight = true;
    private bool immunityFrames = false;

    [Header("Patrol")]
    public Vector3 leftLimit = new Vector3(140f, 15f, 0f);
    public Vector3 rightLimit = new Vector3(170f, 15f, 0f);
    private bool movingRight = true;

    // Internal
    private GameObject player;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        if (!isDead && !isAttacking)
        {
            Patrol();
        }

        AimAtPlayer();
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
            }
        }
        else
        {
            transform.position -= new Vector3(step, 0, 0);
            if (transform.position.x <= leftLimit.x)
            {
                movingRight = true;
            }
        }
    }

    void AimAtPlayer()
    {
        if (player == null || firespot == null) return;

        Vector3 direction = player.transform.position - firespot.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firespot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        animator.SetBool("attack3", true);

        float timer = 0f;
        float duration = 2.5f;

        while (timer < duration)
        {
            Instantiate(bullet1, firespot.transform.position, firespot.transform.rotation);
            yield return new WaitForSeconds(0.03f);
            timer += 0.1f;
        }

        animator.SetBool("attack3", false);
        isAttacking = false;
    }

    void TakeDamage()
    {
        if (!immunityFrames)
        {
            StartCoroutine(FlashDamageColor());
            immunityFrames = true;
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
        sr.color = new Color(1f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
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
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
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
}
