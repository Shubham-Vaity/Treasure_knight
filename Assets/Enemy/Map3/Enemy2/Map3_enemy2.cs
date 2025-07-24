using UnityEngine;
using UnityEngine.UIElements;

public class Map3_enemy2 : MonoBehaviour
{



    public Vector3 speed = new Vector3(1,0,0);
    public Animator animator;
    public Vector2 rayDirection;
    public float rayCastOffSet;
    public float detectionRange;
    public LayerMask playerLayer;


    public bool attacking;



    private void Start()
    {
        
    }

    private void Update()
    {
        
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
}
