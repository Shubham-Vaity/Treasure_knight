using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Explosion Settings")]
    public GameObject explosionPrefab;
    public int minExplosions = 8;
    public int maxExplosions = 10;
    public float explosionRadius = 2f;

    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        StartCoroutine(FlashHitColor());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashHitColor()
    {
        if (spriteRenderer != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(colorResetDelay);
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        isDead = true;

        // Spawn explosions
        int explosionCount = Random.Range(minExplosions, maxExplosions + 1);
        for (int i = 0; i < explosionCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * explosionRadius;
            Instantiate(explosionPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }

        // Optional: Disable visuals before scene switch
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // Switch to scene index 4 after short delay
        StartCoroutine(SwitchSceneAfterDelay(0.5f));
    }

    private IEnumerator SwitchSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(4);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
}
