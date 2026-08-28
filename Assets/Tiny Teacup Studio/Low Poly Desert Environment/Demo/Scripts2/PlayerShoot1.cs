using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot1 : MonoBehaviour
{
    [Header("Combat Settings")]
    public float range = 100f;
    public float damage = 25f;
    public int bulletsLeft = 7; 

    [Header("Gun Reference")]
    public GameObject gun;

    [Header("UI Elements")]
    public GameObject[] bulletImages;
    public Text bulletCounterText;

    [Header("Cinematics & VFX")]
    public KillCam1 killCamScript;
    public GameObject tracerPrefab;
    public ParticleSystem muzzleFlash;
    public AudioSource shootAudio;

    [Header("Legacy Animations")]
    public Animation characterAnimation;
    public Animation aimRecoilAnimation;

    [Header("Crosshair Anims")]
    public Animation topHair, bottomHair, leftHair, rightHair;

    private Reaload1 reloadScript;
    private PlayerHealth1 health;

    void Start()
    {
        reloadScript = GetComponent<Reaload1>();
        health = GetComponent<PlayerHealth1>();
        UpdateUI();
    }

    void LateUpdate()
    {
        if (health != null && health.currentHealth <= 0) return;

        if (gun != null)
        {
            gun.transform.localPosition = new Vector3(0.0318781435f, 0.171724796f, 0.00299198693f);
            gun.transform.localEulerAngles = new Vector3(321.713959f, 96.4691238f, 272.61151f);
        }
    }

    public void Shoot()
    {
        if (bulletsLeft <= 0 || (reloadScript != null && reloadScript.isReloading)) return;

        bulletsLeft--;
        if (bulletsLeft < bulletImages.Length)
        {
            bulletImages[bulletsLeft].SetActive(false);
        }
        UpdateUI();

        if (muzzleFlash != null) muzzleFlash.Play();
        if (shootAudio != null) shootAudio.Play();

        if (aimRecoilAnimation != null) { aimRecoilAnimation.Rewind(); aimRecoilAnimation.Play(); }
        if (characterAnimation != null) { characterAnimation.Rewind("Shoot"); characterAnimation.Play("Shoot"); }

        PlayCrosshairAnim();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            // 1. Check for Standard Enemy (Headshots allowed)
            EnemyHealth1 enemy = hit.transform.GetComponentInParent<EnemyHealth1>();
            if (enemy != null)
            {
                if (hit.transform.CompareTag("EnemyHead"))
                {
                    if (killCamScript != null) killCamScript.StartKillCam(hit.point, enemy);
                }
                else
                {
                    enemy.TakeDamage(damage);
                }
            }

            // 2. Check for Zombie (Standard damage only)
            ZombieAI zombie = hit.transform.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                zombie.health -= damage;
                Debug.Log("Zombie hit! HP: " + zombie.health);
            }
        }
        else
        {
            endPoint = ray.origin + (ray.direction * range);
        }

        if (tracerPrefab != null) SpawnPlayerTracer(ray.origin, endPoint);
    }

    public void UpdateUI()
    {
        if (bulletCounterText != null) bulletCounterText.text = bulletsLeft.ToString();
    }

    void PlayCrosshairAnim()
    {
        if (topHair) { topHair.Rewind(); topHair.Play(); }
        if (bottomHair) { bottomHair.Rewind(); bottomHair.Play(); }
        if (leftHair) { leftHair.Rewind(); leftHair.Play(); }
        if (rightHair) { rightHair.Rewind(); rightHair.Play(); }
    }

    void SpawnPlayerTracer(Vector3 start, Vector3 end)
    {
        GameObject tracer = Instantiate(tracerPrefab, start, Quaternion.identity);
        LineRenderer lr = tracer.GetComponent<LineRenderer>();
        if (lr != null) { lr.SetPosition(0, start); lr.SetPosition(1, end); }
        Destroy(tracer, 0.1f);
    }
}