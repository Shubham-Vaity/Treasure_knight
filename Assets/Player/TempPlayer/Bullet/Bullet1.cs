using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{

    public float bulletSpeed;

    private PlayerMovement player;
    private Gun gun;


    public SpriteRenderer spriteRenderer;
    public Rigidbody2D r2d;



 void Start()
    {
        
        player = GetComponent<PlayerMovement>();
        
        StartCoroutine(fireDealy(1.5f));

    }


    void Update()
    {
        fireindurection();


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

        if (collision.gameObject.CompareTag("Enemy"))
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
