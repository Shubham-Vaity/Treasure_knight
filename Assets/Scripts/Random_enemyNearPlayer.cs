using System.Collections;
using UnityEngine;

public class Random_enemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public LayerMask groundLayer;
    public float rayDistance = 5f;

    public bool start;

    public float minSpawnDelay = 2f;
    public float maxSpawnDelay = 6f;


    public int maxEnemies = 5;


    private void Start()
    {
        StartCoroutine(SpawnEnemiesWithRandomDelay());
    }

    IEnumerator SpawnEnemiesWithRandomDelay()
    {

        while (true)
        {
                if (CountEnemies() < maxEnemies)
                {
                     if (start)
                        {
                    
                    TrySpawnEnemy();
                         }

            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);
                 }
        }
    }
    void TrySpawnEnemy()
    {

        Vector2 rayOrigin = spawnPoint.position + Vector3.up * 2f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundLayer);

        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red, 1f);

        if (hit.collider != null)
        {
            Vector3 spawnPos = new Vector3(spawnPoint.position.x, hit.point.y + 1f, 0);

            Collider2D overlap = Physics2D.OverlapCircle(spawnPos, 0.5f, groundLayer);
            if (overlap == null)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
