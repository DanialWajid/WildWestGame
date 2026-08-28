using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class playerTurnScript : MonoBehaviour
{
    public Animator animator;
    public GameObject gun;

    [Header("UI Reference")]
    public Text duelText; // Drag your UI Text here

    void Start()
    {
        // Ensure gun is hidden and text is ready at the exact moment of start
        if (gun != null) gun.SetActive(false);

        if (duelText != null)
        {
            duelText.gameObject.SetActive(true);
            duelText.text = "";
        }

        StartCoroutine(AnimationSequence());
    }

    IEnumerator AnimationSequence()
    {
        // 1. Initial wait (First 2 seconds of the game)
        yield return new WaitForSeconds(1f);

        // 2. Countdown from 3 to 1
        if (duelText != null) duelText.text = "3";
        yield return new WaitForSeconds(1f);

        if (duelText != null) duelText.text = "2";
        yield return new WaitForSeconds(1f);

        if (duelText != null) duelText.text = "1";
        yield return new WaitForSeconds(1f);

        // 3. The Turn Animation triggers at the 4-second mark
        if (animator != null) animator.SetTrigger("turnAim");

        // 4. DRAW! for 2 seconds
        if (duelText != null)
        {
            duelText.color = Color.yellow;
            duelText.text = "DRAW!";
        }

        // Enable gun during the turn transition
        yield return new WaitForSeconds(0.5f);
        if (gun != null) gun.SetActive(true);
        yield return new WaitForSeconds(1.5f); // Finish the 2-second Draw duration

        // 5. FIRE! for 2 seconds
        if (duelText != null)
        {
            duelText.color = Color.red;
            duelText.text = "FIRE!";
        }
        yield return new WaitForSeconds(2f);

        // Hide text so player can see clearly
        if (duelText != null) duelText.text = "";
    }
}