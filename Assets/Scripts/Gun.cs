using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{


    
    
    public  GameObject gunpoint;
    public GameObject projectile;
    public float bulletDelay = 0.5f;
    

    private bool bulletFired = true;
    private Bullet1 bullet;
    



    void Start()
    {
    
        
        
    }


    void Update()
    {


        if (Input.GetAxis("Fire1") != 0)
        {
            fire();
        }
    }


    public void fire()
    {


        if (bulletFired)
        {
   
            Instantiate(projectile, gunpoint.transform.position, gunpoint.transform.rotation);
            bulletFired = false;

            StartCoroutine(fireDealy(bulletDelay));
        }


    }


    IEnumerator fireDealy(float buttletDelay)
    {

        yield return new WaitForSeconds(buttletDelay);
        bulletFired = true;

    }

}
