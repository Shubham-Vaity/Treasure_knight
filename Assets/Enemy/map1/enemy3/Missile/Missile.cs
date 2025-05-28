using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float homingDuration = 2f; // How long it follows the player
    private bool isHoming = true;

    private float HP = 5;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 lastDirection;
    public SpriteRenderer spriteRenderer;

    public GameObject explosion;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null && spriteRenderer != null)
        {
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }



        StartCoroutine(HomingTimer());
        StartCoroutine(DestroyAfterTime(5f)); // detonation time



    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction;

            if (isHoming)
            {
                direction = (player.position - transform.position).normalized;
            }
            else
            {
                // Keep moving in the last direction after homing stops
                direction = rb.linearVelocity.normalized;
            }

            rb.linearVelocity = direction * bulletSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle; // Adjust based on your sprite's default facing
        }


        if (this.HP <= 0)
        {
            expoad();
        }

    }

     
    public void expoad()
    {
        Instantiate(explosion, this.transform.position, this.transform.rotation);

        Destroy(gameObject);

    }



    IEnumerator HomingTimer()
    {
        yield return new WaitForSeconds(homingDuration);
        isHoming = false; // Stop homing
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        expoad();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor")) 
        {
            expoad();
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            expoad();
            Destroy(gameObject);
        }


        if (collision.gameObject.CompareTag("Bullet"))
        {
            this.HP--;
            
        }
    }

}
