using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("Look Settings")]
    public float sensitivity = 0.2f;
    public Transform playerCamera; // Drag your Camera here in the Inspector

    private float xRotation = 0f;

    void Update()
    {
        // Check if there is a touch moving on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Get how much the finger moved since the last frame
                float mouseX = touch.deltaPosition.x * sensitivity;
                float mouseY = touch.deltaPosition.y * sensitivity;

                // 1. Rotate the Player Body left and right (Y axis)
                transform.Rotate(Vector3.up * mouseX);

                // 2. Rotate the Camera up and down (X axis)
                xRotation -= mouseY;

                // Clamp the rotation so the player can't look behind their own back
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
    }
}