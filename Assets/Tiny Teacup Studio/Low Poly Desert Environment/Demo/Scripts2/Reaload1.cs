using UnityEngine;
using UnityEngine.UI;
using System.Collections; // <--- THIS IS THE MISSING LINE THAT FIXES THE ERROR

public class Reaload1 : MonoBehaviour
{
    [Header("Reload Settings")]
    public float reloadDuration = 1.5f;
    public AudioSource reloadSound;

    [Header("UI Elements")]
    public Text reloadPromptText;

    [Header("Animations")]
    public Animation magAnimation;
    public Animation chamberAnimation;

    public GameObject reloadButton;

    [HideInInspector] public bool isReloading = false;
    private PlayerShoot1 shootScript;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            shootScript = playerObject.GetComponent<PlayerShoot1>();
        }

        if (reloadPromptText != null) reloadPromptText.gameObject.SetActive(false);
        if (reloadButton != null) reloadButton.SetActive(false);
    }

    void Update()
    {
        if (shootScript != null)
        {
            if (shootScript.bulletsLeft <= 0 && !isReloading)
            {
                if (reloadPromptText != null) reloadPromptText.gameObject.SetActive(true);
                if (reloadButton != null) reloadButton.SetActive(true);
            }
            else
            {
                if (reloadPromptText != null) reloadPromptText.gameObject.SetActive(false);
                if (reloadButton != null) reloadButton.SetActive(false);
            }
        }
    }

    public void TriggerReload()
    {
        if (shootScript == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null) shootScript = playerObject.GetComponent<PlayerShoot1>();
        }

        if (shootScript != null && !isReloading && shootScript.bulletsLeft < 7)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (reloadPromptText != null) reloadPromptText.gameObject.SetActive(false);
        if (reloadButton != null) reloadButton.SetActive(false);

        if (reloadSound != null) reloadSound.Play();

        if (magAnimation != null)
        {
            magAnimation.Play("MagRemAnimation");
            yield return new WaitForSeconds(0.4f);
            magAnimation.Play("MagInAnimation");
        }

        if (chamberAnimation != null) chamberAnimation.Play("ChamberAnimation");

        yield return new WaitForSeconds(reloadDuration);

        shootScript.bulletsLeft = 7;
        foreach (GameObject bullet in shootScript.bulletImages)
        {
            if (bullet != null) bullet.SetActive(true);
        }
        shootScript.UpdateUI();

        isReloading = false;
    }
}