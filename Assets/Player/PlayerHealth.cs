using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HPBar healthBar;
    public bool CanTakeDamage = true;

    public GameObject startScreen;
    public GameObject PauseScreen;

 


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


        Time.timeScale = 0f; // Pause game immediately
        StartCoroutine(UnpauseAfterDelay());
    }

    IEnumerator UnpauseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(5f); // Wait 5 real-time seconds
        startScreen.SetActive(false);   
        Time.timeScale = 1f; // Resume game

      
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
         

            PlayerMovement movement = GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.dead = true;
                StartCoroutine(heal());
            }
        }

    }

    IEnumerator heal()
    {
        yield return new WaitForSeconds(0.3f);
        currentHealth = maxHealth;
        TakeDamage(0);
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

   

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damage();
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(heal());
        }
    }




    public void pause()
    {
        Time.timeScale = 0f;
        

        PauseScreen.SetActive(true);
        Debug.Log("Pausw");
    }


    public void unpause()
    {
        Time.timeScale = 1f;
        PauseScreen.SetActive(false);
        Debug.Log("unPausw");
    }


    public void Quit()
    {
        Debug.Log("Q");
        SceneManager.LoadScene(0);
    }
}
