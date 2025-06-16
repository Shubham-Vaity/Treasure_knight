using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{


    
    
    public  GameObject gunpointFrount;
    public  GameObject gunpointTop;
    public  GameObject gunpointBottum;
    GameObject currentGunpoint;
    public GameObject projectile;
    public float bulletDelay = 0.5f;
    

    private bool bulletFired = true;
    private Bullet1 bullet;
    



    void Start()
    {

        currentGunpoint = gunpointFrount;
        
    }


    void Update()
    {

        if (Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                currentGunpoint = gunpointTop;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                currentGunpoint = gunpointBottum;
            }
        }
        else
        {
            currentGunpoint = gunpointFrount;
        }



        if (Input.GetAxis("Fire1") != 0)
        {
            fire();
        }
    }


    public void fire()
    {


        if (bulletFired)
        {
   
            Instantiate(projectile, currentGunpoint.transform.position, currentGunpoint.transform.rotation);
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
