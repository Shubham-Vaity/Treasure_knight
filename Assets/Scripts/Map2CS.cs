using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Map2CS : MonoBehaviour
{

    public PlayableDirector director;


    public GameObject spawn;
    public GameObject boss;

    public bool done;

    

    IEnumerator spawnboss()
    {
        yield return new WaitForSeconds(2.5f);
      
        Instantiate(boss,spawn.transform.position,spawn.transform.rotation);
    }


   


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // Make sure the player has tag "Player"
        {
            if (!done)
            {
                done = true;
            StartCoroutine(spawnboss());
                Random_enemy[] spawners = FindObjectsOfType<Random_enemy>();
                foreach (Random_enemy spawner in spawners)
                {
                    spawner.start = false;

                }

                director.Play();
            
            }
        }
    }
}
