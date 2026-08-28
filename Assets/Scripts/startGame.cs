using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    public void StartGame()
    {
        // Start the sequence to show the ad then load the level
        StartCoroutine(PlayAdThenStart());
    }

    IEnumerator PlayAdThenStart()
    {
        // 1. Check if AdsManager exists and show the ad
        if (AdsManager.Instance != null)
        {
            Debug.Log("Menu: Showing Ad before loading level...");
            AdsManager.Instance.ShowAd();

            // 2. Wait a few seconds to allow the ad to pop up and play
            // This prevents the scene from switching while the ad is starting
            yield return new WaitForSeconds(1.5f);
        }

        // 3. Load the level
        SceneManager.LoadScene("Level1");
    }
}