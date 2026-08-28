using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerDiveScript : MonoBehaviour
{
    public Animator anim;
    public GameObject gun;
    public Transform playerBody;

    private bool isDiving = false;

    public void Dive()
    {
        if (isDiving) return;
        isDiving = true;

        if (gun != null) gun.SetActive(false);

        // Ensure Animator respects Time.timeScale
        anim.updateMode = AnimatorUpdateMode.Normal;
        anim.SetTrigger("diving");

        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

        // Using WaitForSeconds ensures the dive is slow during Slo-Mo
        StartCoroutine(DiveRoutine(1.2f));
    }

    IEnumerator DiveRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        ResetDive();
    }

    void ResetDive()
    {
        isDiving = false;
        if (playerBody != null) playerBody.rotation = Quaternion.Euler(0, 240f, 0);

        if (gun != null)
        {
            gun.SetActive(true);
            gun.transform.localPosition = new Vector3(0.0318781435f, 0.171724796f, 0.00299198693f);
            gun.transform.localEulerAngles = new Vector3(321.713959f, 96.4691238f, 272.61151f);
        }
        anim.ResetTrigger("diving");
    }
}