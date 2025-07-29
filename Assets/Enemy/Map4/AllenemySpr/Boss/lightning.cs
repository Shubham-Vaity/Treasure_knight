using System.Collections;
using UnityEngine;

public class lightning : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AttackLoop());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AttackLoop()
    {
            yield return new WaitForSeconds(1.3f);
            Destroy(gameObject);
     
    }
}
