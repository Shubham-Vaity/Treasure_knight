  using System.Collections;
using UnityEngine;

public class Enemy1_projectile : MonoBehaviour
{

    public float bulletSpeed;




    public SpriteRenderer spriteRenderer;
    public Rigidbody2D r2d;





    void Start()
    {



        StartCoroutine(fireDealy(1.5f));

    }


    void Update()
    {
        fireindurection();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

    }

    public void fireindurection()
    {

        Vector3 speed = new Vector3(bulletSpeed, 0, 0);

        transform.Translate(speed * Time.deltaTime);




    }





    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Floor"))
        {

            Destroy(gameObject);
        }


        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(gameObject);
        }
    }






    IEnumerator fireDealy(float buttletDelay)
    {

        yield return new WaitForSeconds(buttletDelay);

        Destroy(gameObject);

    }
}
