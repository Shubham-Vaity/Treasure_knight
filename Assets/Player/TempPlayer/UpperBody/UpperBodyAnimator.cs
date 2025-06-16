using UnityEngine;

public class UpperBodyAnimator : MonoBehaviour
{

    public  SpriteRenderer SpriteRenderer;
    public Animator animator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("walk",true);   
        }
        else{
            animator.SetBool("walk", false);
        }
        
    }
}
