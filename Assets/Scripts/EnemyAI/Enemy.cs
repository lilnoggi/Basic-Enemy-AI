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

    public float enemyMaxHealth = 100;
    public float enemyCurrentHealth = 0;

    [Header("Enemy Wandering Settings")]
    public float wanderRadius = 10f; // Radius for wandering
    private Vector3 startPos; // Starting position of the enemy
    private float wanderSpeed = 2f; // Speed of wandering
    private float chasingSpeed = 4f; // Speed of chasing
    public NavMeshAgent agent; // Reference to the NavMeshAgent component

    [Header("Enemy Shooting Settings (Enemy 1)")]
    public bool useShooterAI = false;
    public float bulletSpeed = 10f;
    public float bulletDamage = 40f;
    public Transform bulletFirePoint;
    public GameObject bulletPrefab;

    [Header("Enemy Patrol Settings (For Enemy 2)")]
    public bool usePatrolAI = false; // Toggle for using patrol AI
    public Transform[] patrolPoints; // Array of patrol points for Enemy 2
    private int currentPatrolIndex = 0; // Current index in the patrol points array

    [Header("Attack Settings")]
    public float attackDamage = 10f; // Damage dealt to the player
    public float attackRange = 2f; // Range for attacking the player
    public float attackCooldown = 0.5f; // Cooldown time between attacks
    private float nextAttackTime = 0f; // Time until the next attack is allowed

    // === References to Other Scripts === \\
    private EnemyStates enemyStates; // Reference to the EnemyStates script
    private DistanceToPlayer distanceToPlayer; // Reference to the DistanceToPlayer script
    private PlayerHealth playerHealth; // Reference to the PlayerHealth script

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
        enemyCurrentHealth = enemyMaxHealth;

        enemyStates = GetComponent<EnemyStates>();
        distanceToPlayer = GetComponent<DistanceToPlayer>();

        // --- THE FIX ---
        // 1. Check if there is a reference to the player location
        if (distanceToPlayer != null && distanceToPlayer.playerLocation != null)
        {
            // 2. Find the PlayerHealth script specifically ON THE PLAYER OBJECT
            playerHealth = distanceToPlayer.playerLocation.GetComponent<PlayerHealth>();
        }
        // ----------------

        // --- Patrol AI Setup --- \\
        if (usePatrolAI && patrolPoints.Length > 0)
        {
            BubbleSortPatrolPoints();
        }

        InvokeRepeating("Wander", 0, 5f);
    }

    // === Idle State Methods === \\
    // (To be implemented)
    // === End Idle State Methods === \\

    // === Wandering State Methods === \\

    // --- Wander within a specified radius from the start position --- \\
    public void Wander()
    {
        // A simple, safe check is to only set the destination if the agent is not currently stopped.
        if (agent.isStopped == false)
        {
            agent.speed = wanderSpeed; // Ensure the agent's speed is set to wandering speed

            // --- IF ENEMY 2 (PATROL) --- \\
            if (usePatrolAI && patrolPoints.Length > 0)
            {
                agent.speed = 5f;
                // Move to the specific pillar
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);

                // Check if enemy 2 reached the patrol point
                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 1.5f)
                {
                    // Go to the next point
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                }
            }
            // --- IF ENEMY 1 (RANDOM WANDER) --- \\
            else
            { 

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
}
    // === End Wandering State Methods === \\

    // === Detected Player State Methods === \\

    // --- Face the player smoothly --- \\
    public void FacePlayer(DistanceToPlayer player)
    {
        // Face the player
        Vector3 directionToPlayer = (player.playerLocation.position - transform.position).normalized; // Get direction to player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z)); // Create rotation towards player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smoothly rotate towards player
    }

    // --- Wait for a specified number of seconds before switching to Chase state --- \\
    public IEnumerator FaceAndChaseRoutine(DistanceToPlayer player)
    {
        yield return new WaitForSeconds(0.5f); // Wait for 2 seconds

        enemyStates.ChangeState(EnemyStates.State.Chase); // Switch to Chase state
    }

    // === End Detected Player State Methods === \\

    // === Chase Player State Methods === \\

    // --- Chase the player until within attack range --- \\
    public void ChasePlayer()
    {
        // 1. Safety Check: Ensure references exist first
        if (distanceToPlayer == null || distanceToPlayer.playerLocation == null) return;

        // 2. Check Attack Range logic
        // Calculate distance
        float dist = Vector3.Distance(transform.position, distanceToPlayer.playerLocation.position);

        if (dist <= attackRange)
        {
            // If close enough, switch to Attack
            enemyStates.ChangeState(EnemyStates.State.Attack);

            // Stop the agent so they don't push the player
            agent.isStopped = true;
        }
        else
        {
            // 3. If NOT close enough, Chase
            agent.isStopped = false; // Ensure the agent is not stopped
            agent.speed = chasingSpeed; // Set the agent's speed to the chasing speed
            agent.SetDestination(distanceToPlayer.playerLocation.position); // Set destination
        }
    }
    // === End Chase Player State Methods === \\

    // === Attack Player State Methods === \\

    // --- Attack the player and reduce their health --- \\
    public void AttackPlayer(DistanceToPlayer player)
    {
        if (Time.time >= nextAttackTime)
        {
            if (useShooterAI)
            {
                FacePlayer(player);
                EnemyShootGun();
            }
            else
            {
                if (playerHealth != null)
                {
                    Debug.Log("Attacking Player for " + attackDamage + " damage!"); // Log attack action
                    playerHealth.TakeDamage(attackDamage); // Inflict damage to the player
                }
            }
            
            nextAttackTime = Time.time + attackCooldown; // Set the next attack time
        }
    }

    void EnemyShootGun()
    {
        if (bulletPrefab != null && bulletFirePoint != null)
        {
            //Debug.Log("Enemy shot gun!");

            GameObject tempBullet = Instantiate(bulletPrefab, bulletFirePoint.position, bulletFirePoint.rotation);

            Bullet bulletScript = tempBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.speed = bulletSpeed;
                bulletScript.damage = bulletDamage;
            }
        }
    }

    // === End Attack Player State Methods === \\

    // === Bubble Sort Patrol Points Method === \\

    void BubbleSortPatrolPoints()
    {
        int n = patrolPoints.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                // Compare names alphabetically
                if (string.Compare(patrolPoints[j].name, patrolPoints[j + 1].name) > 0)
                {
                    // Swap them
                    Transform temp = patrolPoints[j];
                    patrolPoints[j] = patrolPoints[j + 1];
                    patrolPoints[j + 1] = temp;
                }
            }
        }

        Debug.Log(gameObject.name + " has sorted patrol points:");
    }

    // === End Bubble Sort Patrol Points Method === \\

    public void TakeDamage(float damageAmount)
    {
        Debug.Log(gameObject.name + " took damage: " + damageAmount);
        enemyCurrentHealth -= damageAmount; // Reduce current health by damage amount
        enemyCurrentHealth = Mathf.Clamp(enemyCurrentHealth, 0, enemyMaxHealth); // Clamp health between 0 and maxHealth
        if (enemyCurrentHealth <= 0)
        {
            Die(); // Call Die method if health reaches zero
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    // Gizmos for visualisation
    private void OnDrawGizmosSelected()
    {
        // Attack range visualisation
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
