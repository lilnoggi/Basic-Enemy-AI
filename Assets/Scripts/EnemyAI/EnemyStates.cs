using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    // === ENEMY AI STATES SCRIPT === \\
    // This script manages different states of enemy AI behavior.
    // It includes states like Idle, Patrol, DetectedPlayer, Chase, and Attack.
    // Attach this script to the enemy GameObject in the scene.
    // ================================= \\

    // Define the different states for the enemy AI \\
    public enum  State
    {
        Idle, // Initial state where the enemy is inactive
        Wandering, // State where the enemy is patrolling an area
        DetectedPlayer, // State when the enemy has detected the player
        Chase, // State where the enemy chases the player
        Attack // State where the enemy attacks the player
    }

    public State currentState; // Current state of the enemy AI

    private DistanceToPlayer player; // Reference to the DistanceToPlayer script
    private Enemy enemy; // Reference to the Enemy script
    private Coroutine faceAndChaseCoroutine; // Reference to the FaceAndChase coroutine

    public void Start()
    {
        player = GetComponent<DistanceToPlayer>(); // Get the DistanceToPlayer component
        enemy = GetComponent<Enemy>(); // Get the Enemy component

        currentState = State.Wandering; // Start in Idle state
    }

    public void Update()
    {
        // State machine logic \\
        switch (currentState) // Check the current state
        {
            // Handle each state accordingly \\

            // Handle Idle behavior \\
            case State.Idle:
                // Handle Idle behavior
                break;

            // Handle Wandering behavior \\
            case State.Wandering:
                // The InvokeRepeating is used in Enemy.cs to call Wander periodically.
                // Here, the agent continues moving if it hasn't reached its destination.
                // NOTE: The actual movement is handled by NavMeshAgent, triggered in Enemy.Wander()
                break;

            // Handle Detected Player behavior \\
            case State.DetectedPlayer:
                enemy.agent.isStopped = true; // Stop movement
                enemy.FacePlayer(player); // Only runs the facing rotation for one frame

                // --- START THE COROUTINE ONCE --- \\
                if (faceAndChaseCoroutine == null)
                {
                    faceAndChaseCoroutine = StartCoroutine(enemy.FaceAndChaseRoutine(player)); // Start the FaceAndChase coroutine
                }
                break;

            // Handle Chase behavior \\
            case State.Chase:
                // When in Chase, the coroutine is finished, so clear the reference.
                if (faceAndChaseCoroutine != null)
                {
                    faceAndChaseCoroutine = null;
                }
                enemy.agent.isStopped = false; // Resume agent movement
                enemy.ChasePlayer();
                break;

            // Handle Attack behavior \\
            case State.Attack:
                // Handle Attack behavior
                break;
        }
    }

    // Method to change the current state \\
    public void ChangeState(State newState)
    {
        if (currentState == State.DetectedPlayer && newState != State.DetectedPlayer && faceAndChaseCoroutine != null) // If leaving DetectedPlayer state, stop the coroutine
        {
            StopCoroutine(faceAndChaseCoroutine); // Stop the FaceAndChase coroutine
            faceAndChaseCoroutine = null; // Clear the coroutine reference
        }

        if (enemy.agent != null && enemy.agent.enabled && enemy.agent.isOnNavMesh) // Ensure the agent is valid
        {
            if (newState == State.Wandering) // Resume movement when switching to Wandering state
            {
                enemy.agent.isStopped = false; // Resume agent movement
            }
            else if (newState == State.DetectedPlayer) // Stop movement when switching to DetectedPlayer state
            {
                enemy.agent.isStopped = true; // Stop agent movement
            }
        }

        currentState = newState; // Update the current state
        Debug.Log("AI State Changed To: " + newState.ToString()); // Log the state change
    }
}
