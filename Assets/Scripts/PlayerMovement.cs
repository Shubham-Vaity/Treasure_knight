using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float JumpSpeed;
    private bool isGrounded;
    private bool facingRight = true;

    private int jumpCount = 0;
    public int maxJumps = 2;

    public SpriteRenderer sprite;
    public LayerMask groundLasyer;
    public LayerMask wallLayer;
    public Animator animator;
    public Rigidbody2D R2d;

    public Transform groundCheck;
    public Transform wallCheck; // new
    public float wallCheckDistance = 0.2f;

    private bool isTouchingWall;
    public  bool isGrabbingWall;

    public Transform currentcheckpoint;
    public bool dead;

    void Start()
    {
        R2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentcheckpoint = this.transform;
    }

    void Update()
    {
        movement();

        if (dead)
        {
            transform.position = currentcheckpoint.transform.position;
            dead = false;
        }
    }

    private void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Ground & wall checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLasyer);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallLayer);

        //Debug.DrawRay(wallCheck.position, transform.right * wallCheckDistance, Color.red, 0.2f);

        // Reset jump count
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Wall Grab condition: touching wall + not grounded + moving toward wall
        if (isTouchingWall && !isGrounded && ((horizontalInput > 0 && facingRight) || (horizontalInput < 0 && !facingRight)))
        {

            
            isGrabbingWall = true;

        }
        else
        {
            isGrabbingWall = false;
        }

        // Apply wall grab logic
        if (isGrabbingWall)
        {
            R2d.gravityScale = 0;
            R2d.linearVelocity = Vector2.zero;
         
        }
        else
        {
            R2d.gravityScale = 1;
            R2d.linearVelocity = new Vector2(horizontalInput * Speed, R2d.linearVelocity.y);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            Vector2 jumpDir = Vector2.up;

           

            R2d.linearVelocity = jumpDir.normalized * JumpSpeed;
            jumpCount++;
        }

        // Animations
        animator.SetBool("wallGrab", isGrabbingWall);
        animator.SetBool("isJumping", !isGrounded && !isGrabbingWall);
        animator.SetFloat("Yvalo", R2d.linearVelocity.y);
        animator.SetFloat("Xvalo", Mathf.Abs(horizontalInput));

        // Flip
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.green;
            Vector3 direction = facingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + direction * wallCheckDistance);
        }
    }
}
