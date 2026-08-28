using UnityEngine;

public class Zoom1 : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public float normalFOV = 60f;
    public float zoomedFOV = 30f;
    public float smoothSpeed = 10f;

    private bool isZoomed = false;

    void Start()
    {
        // If you didn't drag the camera in, try to find it on this object
        if (mainCamera == null) mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // Calculate the FOV we want to reach
        float targetFOV = isZoomed ? zoomedFOV : normalFOV;

        // Smoothly change the camera's field of view
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }

    // This is the function linked to your Button
    public void ToggleZoom()
    {
        Debug.Log("Zoom button clicked!");
        isZoomed = !isZoomed;
    }
}