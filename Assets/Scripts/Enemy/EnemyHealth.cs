using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public Text headshotText;
    public Animator anim; // Drag the Animator component here
    public AudioSource deathAudio; // Drag the death sound here
    public Text Won;
    public GameObject exit;
    public GameObject easy;
    public GameObject medium;
    public GameObject hard;
    public GameObject lvl2;
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // 1. Reset the trigger just in case it's still "active"
            anim.ResetTrigger("Hit");

            // 2. Play the state directly at the start (Time 0)
            // "Hit" must be the EXACT name of the gray box in your Animator window
            anim.Play("Hit", -1, 0f);
        }
    }

    // Called for headshots OR when health reaches 0
    public void Die()
    {
        // 1. Force play the Death state immediately
        // Make sure "Death" is the EXACT name of the state in your Animator window
        anim.Play("Death", -1, 0f);

        // 2. Optional: Disable the collider so the bullet doesn't hit a dead body again
        if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;

        if (headshotText != null) headshotText.gameObject.SetActive(false);
        if (Won != null) Won.gameObject.SetActive(true);
        if (exit != null) exit.gameObject.SetActive(true);
        if (easy != null) easy.gameObject.SetActive(true);
        if (medium != null) medium.gameObject.SetActive(true);
        if (hard != null) hard.gameObject.SetActive(true);
        if (lvl2 != null) lvl2.gameObject.SetActive(true);

        if (deathAudio != null) deathAudio.Play();

        // 3. Wait for animation to finish before hiding the object
        Invoke("FinalDisable", 3f);
    }

    void FinalDisable()
    {
        gameObject.SetActive(false);
    }
}