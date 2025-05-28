using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy2 : MonoBehaviour
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




    public void dead()
    {
        bulletFired = false;
        animator.SetBool("dead", true);


        StartCoroutine(deadlydealy(0.3f));



    }







    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Bullet"))
        {
            dead();
        }
    }





    IEnumerator fireDealy(float buttletDelay)
    {

        yield return new WaitForSeconds(buttletDelay);
        animator.SetBool("Attacking", false);
        bulletFired = true;


    }

    IEnumerator deadlydealy(float buttletDelay)
    {
        Vector3 enemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        yield return new WaitForSeconds(buttletDelay);

        Instantiate(explosion, enemyPosition, firespot.transform.rotation);

        Destroy(gameObject);


    }


}

