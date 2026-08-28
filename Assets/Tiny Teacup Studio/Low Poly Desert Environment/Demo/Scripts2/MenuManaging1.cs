using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManaging1 : MonoBehaviour
{
    // These static variables stay in memory across scene reloads
    public static float EnemyAccuracy = 90f;
    public static float EnemyDamage = 25f;

    public GameObject easy;
    public GameObject medium;
    public GameObject hard;
    public GameObject exit;
    public Text difficulty;
    public Text Won;
    public GameObject Zombie;



    private void Start()
    {
            easy.gameObject.SetActive(false);
            medium.gameObject.SetActive(false);
            hard.gameObject.SetActive(false);
            exit.gameObject.SetActive(false);  
            difficulty.gameObject.SetActive(false);
            Won.gameObject.SetActive(false);
        Zombie.gameObject.SetActive(false);

    }
    public void SetEasy()
    {
        EnemyAccuracy = 60f; // 40% chance to miss
        EnemyDamage = 15f;
        RestartGame();
    }

    // Call this from your "Medium" Button
    public void SetMedium()
    {
        EnemyAccuracy = 85f;
        EnemyDamage = 25f;
        RestartGame();
    }

    // Call this from your "Hard" Button
    public void SetHard()
    {
        EnemyAccuracy = 98f;
        EnemyDamage = 30f;
        RestartGame();
    }

    private void RestartGame()
    {
        // IMPORTANT: Reset time scale so you don't start in slow motion
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        // Make sure "Menu" matches the name of your menu scene exactly
        SceneManager.LoadScene("Menu");
    }
}