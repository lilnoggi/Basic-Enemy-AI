using UnityEngine;

public class DistanceToPlayer : MonoBehaviour
{
    // === ENEMY AI DISTANCE TO PLAYER SCRIPT === \\
    // This script calculates the distance between the enemy and the player.
    // It can be used to trigger behaviors when the player is within a certain range.
    // Attach this script to the enemy GameObject in the scene.
    // ============================================= \\

    // Reference to the player's Transform
    public Transform playerLocation;

    // Distance threshold for detection
    public float detectionDistance;

    private EnemyStates enemyStates; // Reference to the EnemyStates script

    private void Start()
    {
        enemyStates = GetComponent<EnemyStates>(); // Get the EnemyStates component
        enemyStates.currentState = EnemyStates.State.Wandering; // Initialize state to Wandering
    }

    public void Update()
    {
        if (playerLocation == null) return; // Safety check

        float distanceToPlayer = Vector3.Distance(transform.position, playerLocation.position); // Calculate distance to player

        // Get the current state of the Brain
        EnemyStates.State currentBrainState = enemyStates.currentState; // Get the current state of the EnemyStates

        if (distanceToPlayer <= detectionDistance) // Player is detected
        {
            // ----------------------------------------------------
            // --- FIX: Only transition if the enemy is in a lower state (Idle or Wandering) ---
            // ----------------------------------------------------
            if (currentBrainState == EnemyStates.State.Idle ||
                currentBrainState == EnemyStates.State.Wandering)
            {
                // Transition to DetectedPlayer state only if the enemy was previously non-combat.
                enemyStates.ChangeState(EnemyStates.State.DetectedPlayer);
            }
            // If the state is already DetectedPlayer, Chase, or Attack, do NOTHING.
            // Let the coroutine inside DetectedPlayer (FaceAndChaseRoutine) handle the next transition.
        }
        else // Player is OUT of detection range
        {
            // ----------------------------------------------------
            // --- FIX: Only transition OUT of Chase/Detected if player leaves ---
            // ----------------------------------------------------
            if (currentBrainState == EnemyStates.State.DetectedPlayer ||
                currentBrainState == EnemyStates.State.Chase ||
                currentBrainState == EnemyStates.State.Attack)
            {
                // If the player leaves, force the AI back to Wandering
                enemyStates.ChangeState(EnemyStates.State.Wandering);
            }
        }
    }

    // Gizmos for visualizing detection range in the editor \\
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set gizmo color to red
        Gizmos.DrawWireSphere(transform.position, detectionDistance); // Draw a wire sphere representing detection range
    }
}
