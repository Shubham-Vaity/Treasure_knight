using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    public GameObject barior1;
    public GameObject barior2;

    public GameObject Boss;
    public GameObject bossSpawnPoint;

    public bool bossSpawned;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            barior1.SetActive(true);
            barior2.SetActive(true);

         
            Random_enemy[] spawners = FindObjectsOfType<Random_enemy>();
            foreach (Random_enemy spawner in spawners)
            {
                spawner.start = false;
             
            }

            if (!bossSpawned)
            {
                Instantiate(Boss, bossSpawnPoint.transform.position, bossSpawnPoint.transform.rotation);
                bossSpawned = true;
            }
        }
    }
}
