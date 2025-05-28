using System.Collections;
using UnityEngine;

public class explosion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(fireDealy());
    }

    // Update is called once per frame
    IEnumerator fireDealy()
    {

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);

    }

}
