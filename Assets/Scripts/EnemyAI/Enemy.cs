using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // === ENEMY AI BEHAVIOR SCRIPT === \\
    // This script defines the behavior of an enemy AI character.
    // It includes methods for different states such as Idle, Wandering, Detected Player, Chase Player, and Attack Player.
    // Attach this script to the enemy GameObject in the scene.
    // ================================= \\

    [Header("Enemy Wandering Settings")]
    public float wanderRadius = 10f; // Radius for wandering
    private Vector3 startPos; // Starting position of the enemy
    private float wanderSpeed = 2f; // Speed of wandering
    private float chasingSpeed = 4f; // Speed of chasing
    public NavMeshAgent agent; // Reference to the NavMeshAgent component

    [Header("Damage Settings")]
    public float attackDamage = 10f; // Damage dealt to the player
    public float attackRange = 2f; // Range for attacking the player

    // === References to Other Scripts === \\
    private EnemyStates enemyStates; // Reference to the EnemyStates script
    private DistanceToPlayer distanceToPlayer; // Reference to the DistanceToPlayer script

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        if (agent != null) // Safety check
        {
            agent.speed = wanderSpeed; // Set the agent's speed to the wandering speed
        }

        startPos = this.transform.position; // Store the starting position
    }

    private void Start()
    {
        enemyStates = GetComponent<EnemyStates>(); // Get the EnemyStates component
        distanceToPlayer = GetComponent<DistanceToPlayer>(); // Get the DistanceToPlayer component

        InvokeRepeating("Wander", 0, 5f); // Call the Wander method every 5 seconds
    }

    // === Idle State Methods === \\
    // (To be implemented)
    // === End Idle State Methods === \\

    // === Wandering State Methods === \\
    public void Wander()
    {
        // A simple, safe check is to only set the destination if the agent is not currently stopped.
        if (agent.isStopped == false)
        {
            agent.speed = wanderSpeed; // Ensure the agent's speed is set to wandering speed
            // 1. Calculate a random point near the start position
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += startPos;

            NavMeshHit hit;
            // 2. Sample the NavMesh to find the closest valid position
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                // 3. Set the destination for the NavMeshAgent
                agent.SetDestination(hit.position);
            }
        }
    }
    // === End Wandering State Methods === \\

    // === Detected Player State Methods === \\
    public void FacePlayer(DistanceToPlayer player)
    {
        // Face the player
        Vector3 directionToPlayer = (player.playerLocation.position - transform.position).normalized; // Get direction to player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z)); // Create rotation towards player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate towards player
    }

    // Wait for a specified number of seconds before switching to Chase state
    public IEnumerator FaceAndChaseRoutine(DistanceToPlayer player)
    {
        yield return new WaitForSeconds(0.5f); // Wait for 2 seconds

        enemyStates.ChangeState(EnemyStates.State.Chase); // Switch to Chase state
    }
    // === End Detected Player State Methods === \\

    // === Chase Player State Methods === \\
    public void ChasePlayer()
    {
        if (distanceToPlayer != null && distanceToPlayer.playerLocation != null)
        {
            agent.isStopped = false; // Ensure the agent is not stopped
            agent.speed = chasingSpeed; // Set the agent's speed to the chasing speed
            agent.SetDestination(distanceToPlayer.playerLocation.position); // Set destination to player's position
        }
    }
    // === End Chase Player State Methods === \\

    // === Attack Player State Methods === \\
    // (To be implemented)
    // === End Attack Player State Methods === \\

    // Gizmos for visualisation
    private void OnDrawGizmosSelected()
    {
        // Attack range visualisation
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
