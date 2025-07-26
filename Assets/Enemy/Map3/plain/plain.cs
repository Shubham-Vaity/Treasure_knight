using System.Collections;
using UnityEngine;

public class plain : MonoBehaviour
{


    public Vector2 speed;
    public GameObject boomb;
    public GameObject firespot;
    public Rigidbody2D r2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        r2d.AddForce(speed);

        StartCoroutine(death());


        InvokeRepeating(nameof(boom), 3f, 3.0f);
     

    }

    // Update is called once per frame
    void Update()
    {
      

    }


    
    void boom()
    {
        Instantiate(boomb, firespot.transform.position, firespot.transform.rotation);
    }


    IEnumerator death()
    {

        yield return new WaitForSeconds(120.1f);
        Destroy(gameObject);
    }


}
