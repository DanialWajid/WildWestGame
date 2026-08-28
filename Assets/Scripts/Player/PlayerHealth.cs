using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Text deathText;
    public AudioSource deathAudio; // Drag your death sound here

    [Header("Components")]
    public Animator anim;

    [Header("Gun Reference")]
    public GameObject gun; // Drag your M9 gun here

    private bool isDead = false;

    [Header("Menu Items")]
    public GameObject easy;
    public GameObject medium;
    public GameObject hard;
    public GameObject exit;
    public Text difficulty;

    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;



    void Start()
    {
        if (deathText != null) deathText.gameObject.SetActive(false);
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 1. Reset and play Hit animation
            anim.ResetTrigger("Hit");
            anim.Play("Hit", -1, 0f);

            // 2. Snap gun to the specific position and rotation you provided
            if (gun != null)
            {
                // Set the specific position
                gun.transform.localPosition = new Vector3(0.0318781435f, 0.171724796f, 0.00299198693f);

                // Set the specific angles to stop it from rotating/twisting
                gun.transform.localEulerAngles = new Vector3(321.713959f, 96.4691238f, 272.61151f);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        anim.Play("Death", -1, 0f);

        // Disable movement and shooting
        if (GetComponent<PlayerMovement>() != null) GetComponent<PlayerMovement>().enabled = false;
        if (GetComponent<PlayerShoot1>() != null) GetComponent<PlayerShoot1>().enabled = false;

        if (deathAudio != null) deathAudio.Play();

        // Show the "You Died" text immediately
        if (deathText != null) deathText.gameObject.SetActive(true);

        // CALL THE AD WITH A SMALL DELAY (gives time for the death animation)
        Invoke("ShowAdDelayed", 0.7f);

        // UI Management
        easy.SetActive(true);
        medium.SetActive(true);
        hard.SetActive(true);
        exit.SetActive(true);
        difficulty.gameObject.SetActive(true);

        // Hide Crosshair
        one.SetActive(false);
        two.SetActive(false);
        three.SetActive(false);
        four.SetActive(false);
    }

    // New helper function for the delay
    void ShowAdDelayed()
    {
        if (AdsManager.Instance != null)
        {
            Debug.Log("PlayerHealth calling ShowAd now...");
            AdsManager.Instance.ShowAd();
        }
        else
        {
            Debug.LogError("AdsManager Instance is null! Is it in the hierarchy?");
        }
    }
}