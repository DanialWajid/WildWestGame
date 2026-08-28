using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using UnityEngine.UI;

public class ZombieAI : MonoBehaviour
{
    [Header("Zombie Stats")]
    public float health = 100f;
    public float attackRange = 1.8f;
    public float runSpeed = 3.5f;
    public float crawlSpeed = 1.2f;

    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;
    private bool hasCrawled = false;
    private bool isDead = false;
    public Text Won;
    public GameObject exit;
    public GameObject easy;
    public GameObject medium;
    public GameObject hard;
    public AudioSource scream;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = runSpeed;

        // Play the scream once
        if (scream != null && scream.clip != null)
        {
            Debug.Log("Scream started!");
        }
        else
        {
            Debug.LogWarning("ZOMBIE ERROR: AudioSource or Clip is missing on the Zombie!");
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Player found! Zombie is coming.");
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        // 1. WOUNDED LOGIC (Health <= 50)
        if (health <= 50 && !hasCrawled)
        {
            hasCrawled = true;
            anim.SetTrigger("Wounded"); // Ensure trigger name matches Animator
            agent.speed = crawlSpeed;
        }

        // 2. DEATH LOGIC (Health <= 0)
        if (health <= 0)
        {
            isDead = true;
            agent.isStopped = true;
            //anim.SetTrigger("stopEverything"); // Freezes zombie
           gameObject.SetActive(false); // Hides zombie immediately (no animation)
            if (Won != null) Won.gameObject.SetActive(true);
            if (exit != null) exit.gameObject.SetActive(true);
            if (easy != null) easy.gameObject.SetActive(true);
            if (medium != null) medium.gameObject.SetActive(true);
            if (hard != null) hard.gameObject.SetActive(true);
            if (scream != null) scream.Stop();
            return;
        }

        // 3. AI NAVIGATION & ATTACK
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            anim.SetTrigger("Attack");

            // Kill the player
            PlayerHealth1 pHealth = player.GetComponent<PlayerHealth1>();
            if (pHealth != null) pHealth.Die();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }
}