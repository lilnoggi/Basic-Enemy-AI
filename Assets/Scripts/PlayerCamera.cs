using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // === PLAYER CAMERA CONTROLLER SCRIPT === \\
    // This script allows the player to control the camera using mouse movement.
    // It handles rotation around the X and Y axes and clamps the vertical rotation to prevent flipping.
    // Attach this script to the main camera in the scene.
    // ======================================== \\

    // === Settings === \\

    public float sensX; // Sensitivity X
    public float sensY; // Sensitivity Y

    public Transform orientation; // Player orientation

    float xRotation; // Rotation around the X axis
    float yRotation; // Rotation around the Y axis

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    private void Update()
    {
        // Get mouse input \\
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX; // Get mouse movement on X axis
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY; // Get mouse movement on Y axis

        // Calculate rotation \\
        yRotation += mouseX; // Update Y rotation
        xRotation -= mouseY; // Update X rotation

        // Clamp vertical rotation \\
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp X rotation to prevent flipping

        // Apply rotation \\
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); // Apply rotation to the camera
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); // Apply Y rotation to the player orientation
    }
}
