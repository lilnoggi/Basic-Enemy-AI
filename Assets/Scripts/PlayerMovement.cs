using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // === PLAYER MOVEMENT CONTROLLER SCRIPT === \\
    // This script allows the player to move using keyboard input.
    // It handles movement in the forward, backward, left, and right directions.
    // Attach this script to the player GameObject in the scene.
    // =========================================== \\

    [Header("Movement Settings")]
    public float playerMovementSpeed; // Speed of walking
    
    public Transform orientation; // Player orientation

    float horizontalInput; // Horizontal input
    float verticalInput; // Vertical input

    Vector3 moveDirection; // Direction of movement

    Rigidbody rb; // Rigidbody component

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.freezeRotation = true; // Freeze rotation to prevent tipping over
    }

    private void Update()
    {
        UserInput(); // Get user input
        MovePlayer(); // Move the player
    }

    private void UserInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right)
        verticalInput = Input.GetAxisRaw("Vertical"); // Get vertical input (W/S or Up/Down)
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // Calculate movement direction
        rb.MovePosition(rb.position + moveDirection.normalized * playerMovementSpeed * Time.fixedDeltaTime); // Move the player
    }
}
