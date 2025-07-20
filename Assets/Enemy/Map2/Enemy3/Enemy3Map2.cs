using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
  
using UnityEngine.PlayerLoop;


public class Enemy3Map2 : MonoBehaviour
{

    public SpriteRenderer _renderer;
    public Animator animator;

    public bool bulletFired;
    public float buletDelay = 1f;
    public float rayCastOffSet;

    public GameObject bullet;
    public GameObject firespot;
    public GameObject explosion;

    public float detectionRange = 5f;
    public LayerMask playerLayer;
    public Vector2 rayDirection;


    public bool Takedamage;
    public float HP;



    //player detection
    private GameObject player;

    private bool facingRight;

    private void Start()
    {



        _renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        bulletFired = true;



        //player detection

        player = GameObject.FindGameObjectWithTag("Player");


    }


    private void Update()
    {
        rotate();
        RaycastCheck();





    }
    private void RaycastCheck()
    {

        Vector2 rayOrigin = (Vector2)transform.position + rayDirection.normalized * rayCastOffSet;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionRange, playerLayer);

        Debug.DrawRay(rayOrigin, rayDirection * detectionRange, Color.red);

        if (hit.collider != null)
        {


            if (hit.collider.CompareTag("Player"))
            {
                if (bulletFired)
                {
                    animator.SetBool("Attacking", true);
                    Instantiate(bullet, firespot.transform.position, firespot.transform.rotation);
                    bulletFired = false;

                    StartCoroutine(animationDelay(0.26f));
                    StartCoroutine(fireDealy(buletDelay));
                }
            }
            else
            {
                animator.SetBool("Attacking", false);
            }
        }
        else
        {
            animator.SetBool("Attacking", false);
        }
    }



    public void rotate()
    {

        //player detection
        Vector3 playerPosition = player.transform.position;
        bool playerIsToTheRight = player.transform.position.x > transform.position.x;
        if (playerIsToTheRight && !facingRight)
        {
            Flip();
            rayDirection = Vector2.right;
        }
        else if (!playerIsToTheRight && facingRight)
        {
            Flip();
            rayDirection = Vector2.left;
        }



        //no glitching
        float flipThreshold = 0.2f;
        float diff = player.transform.position.x - transform.position.x;

        if (diff > flipThreshold && !facingRight)
        {
            Flip();
            rayDirection = Vector2.right;
        }
        else if (diff < -flipThreshold && facingRight)
        {
            Flip();
            rayDirection = Vector2.left;
        }

    }


    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }




    void damage()
    {
        if (!Takedamage)
        {
            Takedamage = true;
            StartCoroutine(ImunityFrames(0.2f));
            HP--;
        }


        if (HP <= 0)
        {
            death();
        }
    }


    IEnumerator ImunityFrames(float buttletDelay)
    {
        animator.SetBool("Damage", true);
        yield return new WaitForSeconds(buttletDelay);

        animator.SetBool("Damage", false);
        Takedamage = false;
    }


    void death()
    {

        animator.SetBool("Dead", true);


        StartCoroutine(deadlydealy(0.1f));
    }








    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Bullet"))
        {
            damage();
        }
    }





    IEnumerator fireDealy(float buttletDelay)
    {

        yield return new WaitForSeconds(buttletDelay);

        bulletFired = true;


    }

    IEnumerator deadlydealy(float buttletDelay)
    {
        Vector3 enemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        yield return new WaitForSeconds(buttletDelay);

        Instantiate(explosion, enemyPosition, firespot.transform.rotation);

        Destroy(gameObject);


    }


    IEnumerator animationDelay(float buttletDelay)
    {

        yield return new WaitForSeconds(buttletDelay);
        animator.SetBool("Attacking", false);



    }

}
