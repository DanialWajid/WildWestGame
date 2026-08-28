using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KillCam1 : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Camera mainCamera;
    public Camera bulletCamera;

    [Header("UI")]
    public GameObject CrossHair;
    public Text killCamText;

    [Header("Audio")]
    public AudioSource killCamAudio;
    public AudioSource windAudio;

    [Header("KillCam Settings")]
    [Range(0.01f, 1f)]
    public float extraSlowMultiplier = 0.2f;
    public float bulletSpeed = 12f;

    [Header("Camera Cinematic")]
    public Vector3 sideOffset = new Vector3(-1.5f, 0.5f, -2f);
    public float zoomFOV = 12f;

    private bool isKillCamActive = false;

    void Start()
    {
        if (killCamText != null)
            killCamText.gameObject.SetActive(false);

        if (bulletCamera != null)
            bulletCamera.gameObject.SetActive(false);
    }

    // UPDATED: Now takes EnemyHealth1 as a parameter
    public void StartKillCam(Vector3 targetPosition, EnemyHealth1 enemyToKill)
    {
        if (isKillCamActive) return;

        StartCoroutine(ExecuteKillCam(targetPosition, enemyToKill));
    }

    // UPDATED: Now uses EnemyHealth1
    IEnumerator ExecuteKillCam(Vector3 target, EnemyHealth1 targetEnemy)
    {
        isKillCamActive = true;

        // Store current timescale
        float previousTimeScale = Time.timeScale;

        // If slo-mo is already active, make killcam even slower
        Time.timeScale = Mathf.Clamp(previousTimeScale * extraSlowMultiplier, 0.01f, 1f);

        // Physics adjustment
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Audio
        if (windAudio != null)
            windAudio.Stop();

        if (killCamAudio != null)
            killCamAudio.Play();

        // UI
        if (killCamText != null)
            killCamText.gameObject.SetActive(true);

        if (CrossHair != null)
            CrossHair.SetActive(false);

        // Setup bullet camera
        bulletCamera.transform.position = mainCamera.transform.position;
        bulletCamera.transform.rotation = mainCamera.transform.rotation;

        mainCamera.gameObject.SetActive(false);
        bulletCamera.gameObject.SetActive(true);

        bulletCamera.fieldOfView = zoomFOV;

        // Spawn bullet
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletCamera.transform.position,
            bulletCamera.transform.rotation
        );

        // Move bullet
        while (bullet != null &&
               Vector3.Distance(bullet.transform.position, target) > 0.1f)
        {
            // Smooth movement even in ultra slow motion
            bullet.transform.position = Vector3.MoveTowards(
                bullet.transform.position,
                target,
                bulletSpeed * Time.unscaledDeltaTime
            );

            // Camera cinematic offset
            Vector3 worldOffset =
                (bullet.transform.right * sideOffset.x) +
                (bullet.transform.up * sideOffset.y) +
                (bullet.transform.forward * sideOffset.z);

            bulletCamera.transform.position =
                bullet.transform.position + worldOffset;

            bulletCamera.transform.LookAt(bullet.transform.position);

            yield return null;
        }

        // UPDATED: Triggers the Die() function in EnemyHealth1
        if (targetEnemy != null)
            targetEnemy.Die();

        // Cleanup bullet
        if (bullet != null)
            Destroy(bullet);

        // Restore ORIGINAL slow motion state
        Time.timeScale = previousTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Restore cameras
        bulletCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Restore UI
        if (CrossHair != null)
            CrossHair.SetActive(true);

        if (killCamText != null)
            killCamText.gameObject.SetActive(false);

        // Restore audio
        if (windAudio != null)
            windAudio.Play();

        isKillCamActive = false;
    }
}