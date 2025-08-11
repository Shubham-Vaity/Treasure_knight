using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Projectile : MonoBehaviour
{

    public float bulletSpeed;


    public AudioClip sound;

    public SpriteRenderer spriteRenderer;
    public Rigidbody2D r2d;



    

    void Start()
    {

        AudioSource.PlayClipAtPoint(sound, transform.position);

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

            StartCoroutine(dealy());
        }


        if (collision.gameObject.CompareTag("Player"))
        {

            StartCoroutine(dealy());
        }
    }






    IEnumerator fireDealy(float buttletDelay)
    {

        yield return new WaitForSeconds(buttletDelay);

        Destroy(gameObject);

    }
    IEnumerator dealy()
    {

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);

    }
}
