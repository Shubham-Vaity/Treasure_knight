using UnityEngine;
using UnityEngine.PlayerLoop;

public class bomb : MonoBehaviour
{

    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Floor"))
        {

            Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
        }

    }
}
