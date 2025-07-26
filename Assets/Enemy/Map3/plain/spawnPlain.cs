using UnityEngine;

public class spawnPlain : MonoBehaviour
{




    // Start is called once before the first execution of Update after the MonoBehaviour is created



    public GameObject plainn;
    void Start()
    {
        InvokeRepeating(nameof(plain), 3.5f, 4.0f);

    }

  
    void plain()
    {
        Instantiate(plainn, this.transform.position, this.transform.rotation);
    }


}
