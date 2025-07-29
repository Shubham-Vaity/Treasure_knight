using System.Collections;
using UnityEngine;

public class spawnLightning : MonoBehaviour
{
    
    public GameObject lightning;
    public GameObject firespot;


    void Start()
    {
        StartCoroutine(AttackLoop());

    }


    void rnd()
    {
        int randomNumber = Random.Range(0, 100);
        if(randomNumber > 70)
        {
            Instantiate(lightning, firespot.transform.position, firespot.transform.rotation);
        }
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(2f);
        rnd();

    }
}
