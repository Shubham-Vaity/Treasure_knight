using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Damage Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float colorResetDelay = 0.1f;

    [Header("Immunity Settings")]
    public float immunityDuration = 0.5f;
    private bool isImmune = false;

    [Header("Explosion Settings")]
    public GameObject explosionPrefab;
    public int minExplosions = 8;
    public int maxExplosions = 10;
    public float explosionRadius = 2f;
    public float explosionDelay = 0.2f; // Delay between each explosion

    private Color originalColor;

    public AudioClip sound;
    void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(FlashHitColor());
        if (isDead || isImmune) return;


        AudioSource.PlayClipAtPoint(sound, transform.position);
        currentHealth -= damage;
        StartCoroutine(ActivateImmunity());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashHitColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(colorResetDelay);
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator ActivateImmunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
    }

    private void Die()
    {
        isDead = true;
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        StartCoroutine(SpawnExplosionsAndChangeScene());
    }

    private IEnumerator SpawnExplosionsAndChangeScene()
    {
        int explosionCount = Random.Range(minExplosions, maxExplosions + 1);

        for (int i = 0; i < explosionCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * explosionRadius;
            Instantiate(explosionPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            yield return new WaitForSeconds(explosionDelay);
        }

        // After explosions, change scene
        SceneManager.LoadScene(5);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
