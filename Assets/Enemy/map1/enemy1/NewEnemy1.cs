using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewEnemy1 : MonoBehaviour
{
    
    public Animator animator;
   

    public GameObject fireSPot;
    public GameObject WallChecker;
    public GameObject explosion;

    public GameObject Bullet;

    public Vector3 speed =new Vector3(4f,0f,0f);


    public bool isattacking;
    public bool FacingRight;

    public LayerMask wallLayer;

    public GameObject player;


    void Start()
    {
        isattacking=true;



        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
            attack();
       
        rotate();
        selfDistruct();


        transform.Translate(speed*Time.deltaTime);
    }


    void attack()
    {
        if (isattacking)
        {
            isattacking = false;
            speed.x = 0;
            animator.SetBool("attack", true);
            Instantiate(Bullet, fireSPot.transform.position,fireSPot.transform.rotation);
            StartCoroutine(attackdelay());
        }
        else
        {
            animator.SetBool("attack", false);
            speed.x = 4;
        }
    }

   
    public void rotate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= 5f)
        {
            float directionToPlayer = player.transform.position.x - transform.position.x;

            if (directionToPlayer > 0 && !FacingRight)
            {
                Flip();
            }
            else if (directionToPlayer < 0 && FacingRight)
            {
                Flip();
            }
        }

        else
        {
            checkWall();
        }

    }

    void selfDistruct()
    {

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer >= 25f)
        {
            float directionToPlayer = player.transform.position.x - transform.position.x;

            if (directionToPlayer > 20 && !FacingRight)
            {
                Destroy(gameObject);
            }
            else if (directionToPlayer < 20 && FacingRight)
            {
                Destroy(gameObject);
            }
        }
    }


    void checkWall()
    {
        Vector2 direction = transform.right;


        RaycastHit2D hit = Physics2D.Raycast(WallChecker.transform.position, direction, 2f, wallLayer);

      

        //Debug.DrawRay(this.transform.position, direction * 2, Color.red, 2f);


        if (hit.collider != null)
        {
            Flip();
        }
      

    

    }

    void death()
    {

        animator.SetBool("Dead", true);


        StartCoroutine(deadlydealy(0.1f));
    }



    IEnumerator attackdelay()
    {

        yield return new WaitForSeconds(3f);

        isattacking=true;
            
    }


    IEnumerator deadlydealy(float buttletDelay)
    {
        Vector3 enemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        yield return new WaitForSeconds(buttletDelay);

        Instantiate(explosion, enemyPosition, this.transform.rotation);

        Destroy(gameObject);


    }


    
    void Flip()
    {

        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            death();
        }
    }



}
