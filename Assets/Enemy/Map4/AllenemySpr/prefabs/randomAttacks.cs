using UnityEngine;

public class RandomShooter2D : MonoBehaviour
{
    public GameObject bulletPrefab;   
    public GameObject bomb;   
    public float bulletSpeed = 10f;   
    public float spreadAngle = 30f;   
    public float shootInterval = 1f;  

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        int choice = Random.Range(0, 2); // 0 = single, 1 = spread

        if (choice == 0)
            SingleShot();
        else
            SpreadShot();
    }

    void SingleShot()
    {
        GameObject bullet = Instantiate(bomb, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * bulletSpeed;
        }
    }

    void SpreadShot()
    {
        int bulletCount = 6;
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle) * transform.rotation;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = rotation * Vector3.right * bulletSpeed;
            }
        }
    }
}
