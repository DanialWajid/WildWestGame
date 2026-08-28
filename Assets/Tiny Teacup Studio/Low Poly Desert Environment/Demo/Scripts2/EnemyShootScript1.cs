using UnityEngine;

public class EnemyShootScript1 : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;
    public float detectionRange = 20f;

    [Header("Weapon Stats (Overrides by Difficulty)")]
    public float fireRate = 1f;
    // We will use the static values instead of these public ones

    [Header("Visuals")]
    public Animator anim;
    public GameObject tracerPrefab;

    private float nextFireTime;
    private PlayerHealth1 playerHealthScript;
    public AudioSource shootAudio;

    void Start()
    {
        // Small delay at start so you aren't shot immediately on spawn
        nextFireTime = Time.time + 10f;

        if (player != null)
        {
            playerHealthScript = player.GetComponent<PlayerHealth1>();
        }
    }

    void Update()
    {
        if (player == null || playerHealthScript == null) return;
        if (playerHealthScript.currentHealth <= 0) return;

        // Sync animator speed with SloMo
        if (anim != null) anim.speed = (Time.timeScale < 1f) ? 1f / Time.timeScale : 1f;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
    }

    void Shoot()
    {
        if (playerHealthScript.currentHealth <= 0) return;
        if (shootAudio != null) shootAudio.Play();

        // Use the STATIC accuracy from MenuManaging
        float spreadValue = (100f - MenuManaging.EnemyAccuracy) / 10f;
        Vector3 fireDirection = player.position - transform.position;

        // Apply spread
        fireDirection.x += Random.Range(-spreadValue, spreadValue);
        fireDirection.y += Random.Range(-spreadValue, spreadValue);
        fireDirection.z += Random.Range(-spreadValue, spreadValue);

        Vector3 rayOrigin = transform.position + Vector3.up;
        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(rayOrigin, fireDirection, out hit, detectionRange))
        {
            endPoint = hit.point;
            if (hit.transform.CompareTag("Player"))
            {
                // Use the STATIC damage from MenuManaging
                playerHealthScript.TakeDamage(MenuManaging.EnemyDamage);
            }
        }
        else
        {
            endPoint = rayOrigin + (fireDirection.normalized * detectionRange);
        }

        if (tracerPrefab != null) SpawnTracer(rayOrigin, endPoint);
    }

    void SpawnTracer(Vector3 start, Vector3 end)
    {
        GameObject tracer = Instantiate(tracerPrefab, start, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }
        float lifeTime = (Time.timeScale < 1f) ? 0.6f : 0.1f;
        Destroy(tracer, lifeTime);
    }
}