using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // === MOVE CAMERA TO TARGET POSITION SCRIPT === \\
    // This script moves the camera to a specified target position every frame.
    // Attach this script to the camera GameObject.
    // ============================================== \\

    // === Target Position === \\
    public Transform cameraPosition;

    private void Update()
    {
        // Move camera to target position \\
        transform.position = cameraPosition.position; // Update camera position to match target position
    }
}
