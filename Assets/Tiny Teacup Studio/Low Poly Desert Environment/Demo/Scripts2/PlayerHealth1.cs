using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth1 : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Text deathText;
    public AudioSource deathAudio;

    [Header("Components")]
    public Animator anim;

    [Header("Gun Reference")]
    public GameObject gun;

    private bool isDead = false;

    [Header("Menu Items")]
    public GameObject easy;
    public GameObject medium;
    public GameObject hard;
    public GameObject exit;
    public Text difficulty;

    [Header("UI Crosshair Pieces")]
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;

    [Header("Zombie Jump Scare")]
    public Transform zombieTransform; // Drag your Zombie object here

    void Start()
    {
        if (deathText != null) deathText.gameObject.SetActive(false);
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.ResetTrigger("Hit");
            anim.Play("Hit", -1, 0f);

            if (gun != null)
            {
                gun.transform.localPosition = new Vector3(0.0318781435f, 0.171724796f, 0.00299198693f);
                gun.transform.localEulerAngles = new Vector3(321.713959f, 96.4691238f, 272.61151f);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 1. Play Death Animation
        anim.Play("Death", -1, 0f);

        // 2. Snap Camera to Zombie (The "Killer View")
        if (zombieTransform != null)
        {
            Camera.main.transform.LookAt(zombieTransform.position + Vector3.up * 1.5f); // Look at zombie's chest/head
            Camera.main.fieldOfView = 30f; // Force zoom for the jump scare
        }

        // 3. Disable movement and shooting
        // (Make sure to disable your PlayerShoot1 script here too!)
        if (GetComponent<PlayerShoot1>() != null) GetComponent<PlayerShoot1>().enabled = false;

        // 4. Play Audio and Show Menus
        if (deathAudio != null) deathAudio.Play();

        if (deathText != null) deathText.gameObject.SetActive(true);
        if (AdsManager.Instance != null) AdsManager.Instance.ShowAd();

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
}