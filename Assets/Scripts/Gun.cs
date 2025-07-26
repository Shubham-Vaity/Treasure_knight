using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject gunpointFrount;
    public GameObject gunpointTop;
    public GameObject gunpointBottum;
    GameObject currentGunpoint;
    public GameObject projectile;
    public float bulletDelay = 0.5f;

    private bool bulletFired = true;

    private PlayerMovement player; // ← reference to your movement script

    void Start()
    {
        currentGunpoint = gunpointFrount;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Check if player is NOT in air or wall grab before aiming up/down
        if (player.isGrounded) // You can change this to !player.isJumping && !player.isFalling if needed
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    currentGunpoint = gunpointTop;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    currentGunpoint = gunpointBottum;
                }
            }
            else
            {
                currentGunpoint = gunpointFrount;
            }
        }
        else
        {
            // Always shoot forward in air/wall
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
