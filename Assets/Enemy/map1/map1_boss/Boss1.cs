using System.Collections;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [Header("References")]
    public GameObject firespot;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public Animator animator;


    [Header("Settings")]
    public float speed = 2f;
    public int HP = 10;
    public float bulletDelay = 0.5f;

    [Header("State")]
    public bool movinRight = true;
    private bool facingRight = true;
    private bool attack = true;
    private bool isDead = false;
    private bool imunityframes = false;


    // Internal
    private Vector3 leftpos = new Vector3(145.0f, 15.0f, 0.0f);
    private Vector3 rightpos = new Vector3(175.0f, 15.0f, 0.0f);
    private GameObject player;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bullet2.transform.localScale = new Vector3(2f, 2f, 1f);
        bullet3.transform.localScale = new Vector3(4f, 4f, 1f);



        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!isDead && attack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        attack = false;
        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                yield return StartCoroutine(Attack1Routine());
                break;
            case 1:
                yield return StartCoroutine(Attack2Routine());
                break;
            case 2:
                yield return StartCoroutine(Attack3Routine());
                break;
            case 3:
                yield return StartCoroutine(DashRoutine());
                break;
        }

        yield return new WaitForSeconds(1f);
        attack = true;
    }

    IEnumerator Attack1Routine()
    {
        animator.SetBool("attack1", true);
        rotate();
        for (int i = 0; i < 3; i++)
        {
            Instantiate(bullet2, firespot.transform.position, firespot.transform.rotation);
            yield return new WaitForSeconds(bulletDelay);
        }
        animator.SetBool("attack1", false);
    }


    IEnumerator Attack2Routine()
    {
        animator.SetBool("attack2", true);
        rotate();
        float[] angles = { 0, 20, -20, 40, -40 };
        foreach (float angle in angles)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle) * firespot.transform.rotation;
            Instantiate(bullet3, firespot.transform.position, rotation);
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("attack2", false);
    }


    IEnumerator Attack3Routine()
    {
        animator.SetBool("attack3", true);
        rotate();
        float timer = 0f;
        float duration = 2f;

        while (timer < duration)
        {
            Instantiate(bullet1, firespot.transform.position, firespot.transform.rotation);
            yield return new WaitForSeconds(0.03f);
            timer += 0.1f;
        }
        animator.SetBool("attack3", false);
    }


    IEnumerator DashRoutine()
    {
        animator.SetFloat("move", 0f);
        rotate();
        yield return new WaitForSeconds(0.5f);

        float dashTime = Random.Range(3f, 5f);
        float timer = 0f;
        animator.SetFloat("move", 1f);

        while (timer < dashTime)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);

            if (!movinRight && this.transform.position.x <= leftpos.x)
            {
                Flip();
                movinRight = true;
            }
            else if (movinRight && this.transform.position.x >= rightpos.x)
            {
                Flip();
                movinRight = false;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetFloat("move", 0f);
    }


    void rotate()
    {
        if (player == null) return;

        bool playerRight = player.transform.position.x > transform.position.x;

        if (playerRight && !facingRight)
            Flip();
        else if (!playerRight && facingRight)
            Flip();
    }

    void Flip()
    {

        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void takedamage()
    {
        StartCoroutine(FlashDamageColor());
        if (!imunityframes)
        {
            imunityframes = true;
            HP--;


            if (HP <= 0)
            {
                if (!isDead)
                {

                    StartCoroutine(death());
                }

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


    IEnumerator death()
    {
        isDead = true;
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }



    IEnumerator DamageCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        imunityframes = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            takedamage();
        }
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                float horizontalForce = 50f;
                float verticalForce = 5f;

                // Calculate direction from boss to player, then reverse it
                Vector2 direction = (collision.transform.position - transform.position).normalized;

                // Make sure it's pushing away from boss horizontally
                Vector2 knockbackDir = new Vector2(direction.x, 0.5f).normalized;

                Vector2 force = new Vector2(knockbackDir.x * horizontalForce, knockbackDir.y * verticalForce);

                playerRb.linearVelocity = Vector2.zero; // Optional: Reset velocity for consistent force
                playerRb.AddForce(force, ForceMode2D.Impulse);

                Debug.Log("Player knocked away from boss.");
            }
        }

    }

}