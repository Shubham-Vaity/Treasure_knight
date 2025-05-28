using UnityEngine;
using System.Collections;

public class Boss1 : MonoBehaviour
{
    public float speed = 3f;
    public float stopDistance = 3f;
    public float attackCooldown = 3f;

    private Transform player;
    private Animator anim;
    private bool isDead = false;
    private bool attack = false;
    private Coroutine currentAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        StartCoroutine(AttackCooldown());
    }

    void Update()
    {
        if (isDead || player == null) return;

        Vector2 direction = player.position - transform.position;
        transform.localScale = new Vector3(direction.x > 0 ? 3 : -3, 3, 1); // Adjust scale flip

        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
            if (attack && currentAttack == null)
            {
                currentAttack = StartCoroutine(PerformAttack());
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(attackCooldown);
            attack = true;
        }
    }

    IEnumerator PerformAttack()
    {
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        attack = false;
        currentAttack = null;
    }

    public void TakeDamage()
    {
        if (isDead) return;
        isDead = true;
        anim.SetTrigger("death");
        Destroy(gameObject, 1.5f);
    }
}
