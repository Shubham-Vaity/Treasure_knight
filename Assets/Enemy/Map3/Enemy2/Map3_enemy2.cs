using UnityEngine;
using UnityEngine.UIElements;

public class Map3_enemy2 : MonoBehaviour
{

    public LayerMask playerLayer;
    public Animator animator;
    public Rigidbody2D r2d;


    public float speed;
    public Vector2 rayDirection;


    public Transform pointA;
    public Transform pointB; 
    public Vector3 CurrentPoint;


    public float rayCastOffSet;
    public float detectionRange;


    public bool facingRight;
    public bool attacking;



    private void Start()
    {
        CurrentPoint = pointA.position;

        InvokeRepeating("RaycastCheck", 0f, 0.1f);
    }

    private void Update()
    {
        move();
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
                attacking = true;
                animator.SetBool("attack", true);
            }
            else
            {
                animator.SetBool("attack", false);
            }
        }
        else
        {
            animator.SetBool("attack", false);
        }
    }



    void move()
    {
        if (CurrentPoint != null) {

            transform.position = Vector3.MoveTowards(transform.position, CurrentPoint, speed * Time.deltaTime);

            if (transform.position == CurrentPoint)
            {
                CurrentPoint = (CurrentPoint == pointA.position) ? pointB.position : pointA.position;
                Flip();
            }

        }

    }



    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }




}
