using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy1 : MonoBehaviour
{


    public SpriteRenderer _renderer;
    public Animator animator;
    public GameObject explosion;
    public GameObject firespot;
    public GameObject bullet;


    public bool attack;
    public bool isattacking;


    private Vector3 speed;
    private int randomNumber;
    private float moveSpeed = 8f;


    public LayerMask wallLayer;
    public Transform wallCheck;
    public float wallCheckDistance = 4f;
    public bool facingRight;



    public float maxDistanceFromPlayer = 5f;
    private Transform player;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        speed = new Vector3(moveSpeed, 0, 0);

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);


        player = GameObject.FindGameObjectWithTag("Player")?.transform;

    }


    void Update()
    {

       // randomAttack();
        selfDistruct();

       WallDetectAndFlip();
        transform.Translate(speed * Time.deltaTime);

    }



    void randomAttack()
    {



        if (attack)
        {

            Instantiate(bullet, firespot.transform.position, firespot.transform.rotation);
            speed.x = 0;
            animator.SetBool("attack", true);

            attack = false;
            StartCoroutine(AttaCkDelay(0.4f));
        }
        else
        {
            randomNumber = Random.Range(1, 100);
            if (!isattacking)
            {

                if (randomNumber == 10)
                {
                    isattacking = true;
                   attack = true;
               }
            }

        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

    }
    void WallDetectAndFlip()
    {
        Vector2 direction = transform.right;


        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, direction, wallCheckDistance, wallLayer);



        if (hit.collider != null)
        {
            Flip();
        }
    }


    void selfDistruct()
    {


        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        float yDistance = Mathf.Abs(transform.position.y - player.position.y);

        if (xDistance > maxDistanceFromPlayer || yDistance > maxDistanceFromPlayer)
        {

            Destroy(gameObject);
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        speed.x *= -1;


        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += 180f;
        transform.eulerAngles = currentRotation;
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
            death();
        }
    }





    IEnumerator AttaCkDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("attack", false);
        speed.x = 8;
        isattacking = false;
    }




    IEnumerator deadlydealy(float buttletDelay)
    {
        Vector3 enemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        yield return new WaitForSeconds(buttletDelay);

        Instantiate(explosion, enemyPosition, this.transform.rotation);

        Destroy(gameObject);


    }

}