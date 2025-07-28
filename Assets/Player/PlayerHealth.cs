using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HPBar healthBar;
    public bool CanTakeDamage = true;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HPBar>();
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
        Debug.Log("Health: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            // Add death logic here if needed
            Debug.Log("Player Dead");
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    void damage()
    {
        if (CanTakeDamage)
        {
            CanTakeDamage = false;
            TakeDamage(4);
            StartCoroutine(DamageDelay());
        }
    }

    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(0.3f);
        CanTakeDamage = true;
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damage();
        }
    }
}
