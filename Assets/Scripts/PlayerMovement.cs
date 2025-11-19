using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // === PLAYER MOVEMENT CONTROLLER SCRIPT === \\
    // This script allows the player to move using keyboard input.
    // It handles movement in the forward, backward, left, and right directions.
    // Attach this script to the player GameObject in the scene.
    // =========================================== \\

    [Header("Movement Settings")]
    public float playerMovementSpeed; // Player's Speed
    public float playerWalkSpeed = 2; // Player's Walk Speed
    public float playerSprintSpeed = 5; // Player's Sprint Speed

    public Transform orientation; // Player orientation

    float horizontalInput; // Horizontal input
    float verticalInput; // Vertical input

    Vector3 moveDirection; // Direction of movement

    Rigidbody rb; // Rigidbody component

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.freezeRotation = true; // Freeze rotation to prevent tipping over
        playerMovementSpeed = playerWalkSpeed; // Initialize movement speed to walk speed
    }

    private void Update()
    {
        PlayerMoveInput(); // Get user input
        MovePlayer(); // Move the player

        // Sprint Input \\
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerSprint(); // Start sprinting
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerMovementSpeed = playerWalkSpeed; // Stop sprinting, revert to walk speed
        }

        Debug.Log(playerMovementSpeed); // Debug log for movement speed
    }

    // Get player movement input \\
    private void PlayerMoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right)
        verticalInput = Input.GetAxisRaw("Vertical"); // Get vertical input (W/S or Up/Down)

        // Normalize diagonal movement \\
        if (horizontalInput != 0 && verticalInput != 0)
        {
            horizontalInput *= 0.7071f; // Scale horizontal input
            verticalInput *= 0.7071f; // Scale vertical input
        }
    }

    // Move the player based on input \\
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // Calculate movement direction
        rb.MovePosition(rb.position + moveDirection.normalized * playerMovementSpeed * Time.fixedDeltaTime); // Move the player
    }

    // Set player to sprinting \\
    private void PlayerSprint()
    {
        playerMovementSpeed = playerSprintSpeed; // Set movement speed to sprint speed
    }
}
