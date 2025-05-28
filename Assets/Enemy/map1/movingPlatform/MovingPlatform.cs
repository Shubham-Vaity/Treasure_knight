using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform pointA;
      public Transform pointB;
    public float speed=2f;


    private Vector3 nextPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPoint=pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=Vector3.MoveTowards(transform.position,nextPoint,speed*Time.deltaTime);

        if (transform.position == nextPoint)
        {
            nextPoint= (nextPoint==pointA.position)? pointB.position:pointA.position;  
        }


    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject != null && collision.gameObject.transform != null)
            {
                collision.gameObject.transform.SetParent(transform);
            }

        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject != null && collision.gameObject.transform != null)
            {
                collision.gameObject.transform.SetParent(null);
            }

        }
    }
}
