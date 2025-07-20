using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class enemy2Map2 : MonoBehaviour
{

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    
    public bool bulletFired;

    public Animator animator;

    public GameObject explosion;
    public GameObject firespot;
    public GameObject bullet;


    public float detectionRange = 5f;
    public float rayCastOffSet = 1f;
    public LayerMask playerLayer;
    public Vector2 rayDirection;



    private Vector3 nextPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPoint = pointB.position;
        bulletFired=true;

        InvokeRepeating("RaycastCheck", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);

        if (transform.position == nextPoint)
        {
            nextPoint = (nextPoint == pointA.position) ? pointB.position : pointA.position;
        }


    }



    void death()
    {

        animator.SetBool("Dead", true);


        StartCoroutine(deadlydealy(0.1f));
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

                    StartCoroutine(attackdelay());
                   
                }
            }
            
        }
        
    }





    IEnumerator attackdelay()
    {

        yield return new WaitForSeconds(0.1f);

        bulletFired = true;

    }


    IEnumerator deadlydealy(float buttletDelay)
    {
        Vector3 enemyPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        yield return new WaitForSeconds(buttletDelay);

        Instantiate(explosion, enemyPosition, this.transform.rotation);

        Destroy(gameObject);


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            death();

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            death();

        }
    }


}
