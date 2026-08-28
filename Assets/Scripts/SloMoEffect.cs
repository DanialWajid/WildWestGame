using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SloMoEffect : MonoBehaviour
{
    public float slowMoScale = 0.2f;
    public Animator playerAnim;

    [Header("UI Elements")]
    public GameObject grayOverlay;
    public Text countdownText;

    private bool isSlowMoActive = false;
    public AudioSource sloMoAudio;
    public AudioSource normalAudio;

    public void Start()
    {
        if (grayOverlay != null) grayOverlay.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    public void TriggerSloMo()
    {
        if (isSlowMoActive) return;
        if (normalAudio != null) normalAudio.Stop();

        if (sloMoAudio != null)
        {
            sloMoAudio.loop = true;
            sloMoAudio.Play();
        }
        StartCoroutine(SloMoRoutine());
    }

    IEnumerator SloMoRoutine()
    {
        isSlowMoActive = true;

        // 1. Set Global Slow Motion
        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        if (playerAnim != null) playerAnim.speed = 1f / slowMoScale;

        if (grayOverlay != null) grayOverlay.SetActive(true);
        if (countdownText != null) countdownText.gameObject.SetActive(true);

        float timeLeft = 10f;
        while (timeLeft > 0)
        {
            if (countdownText != null)
                countdownText.text = timeLeft.ToString("F0");

            // Counts down in real-seconds regardless of game speed
            timeLeft -= Time.unscaledDeltaTime;
            yield return null;
        }

        StopSloMo();
    }

    void StopSloMo()
    {
        isSlowMoActive = false;
        if (sloMoAudio != null) sloMoAudio.Stop();
        if (normalAudio != null) normalAudio.Play();

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        if (playerAnim != null) playerAnim.speed = 1f;

        if (grayOverlay != null) grayOverlay.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }
}