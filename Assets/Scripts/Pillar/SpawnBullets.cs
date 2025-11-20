using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    // === SPAWN BULLETS === \\
    // This script is intended to handle the spawning of bullet objects
    // around a pillar in the game.
    // Bullets are spawned at random positions within a defined radius
    // and height, ensuring they do not overlap with existing pillars.
    // Attach this script to the pillar Prefab.
    // ======================== \\

    public GameObject bulletPrefab; // Prefab of the bullet to spawn
    public int numToSpawn = 1; // Number of bullets to spawn
    public float spawnRadius = 1f; // Radius within which to spawn bullets
    public float spawnHeight = 1f; // Height at which to spawn bullets
    public LayerMask pillarLayer; // Layer mask to check for pillar collisions
    public float bulletCheckRadius = 0.5f; // Radius to check for overlapping pillars

    void Start()
    {
        SpawnNumOfBullets(); // Spawn the specified number of bullets at the start
    }

    // --- Spawn the specified number of bullets --- \\
    void SpawnNumOfBullets()
    {
        const int maxAttempts = 100; // Maximum attempts to find a valid position

        // Loop to spawn the specified number of bullets \\
        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 spawnPos = Vector3.zero; // Initialize spawn position
            bool positionFound = false; // Flag to indicate if a valid position was found

            // Try to find a valid spawn position \\
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                spawnPos = GetRandomSpawnPosition(); // Get a random spawn position

                // Check if the position is clear of pillars \\
                if (IsPositionClear(spawnPos))
                {
                    positionFound = true; // Valid position found
                    break; // Exit the attempt loop
                }
            }

            // Spawn the bullet if a valid position was found \\
            if (positionFound)
            {
                Instantiate(bulletPrefab, spawnPos, Quaternion.identity); // Spawn the bullet at the valid position
            }
            else // Log a warning if no valid position was found \\
            {
                Debug.LogWarning("Could not find a valid spawn position for bullet after maximum attempts.");
            }
        }
    }

    // --- Get a random spawn position within the defined radius and height --- \\
    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomPoint2D = Random.insideUnitCircle * spawnRadius; // Random point in 2D circle

        Vector3 center = transform.position; // Center position of the pillar

        Vector3 randomPosition = center + new Vector3(randomPoint2D.x, spawnHeight, randomPoint2D.y); // Convert to 3D position

        return randomPosition; // Return the random spawn position
    }

    // --- Check if the spawn position is clear of pillars --- \\
    private bool IsPositionClear(Vector3 position)
    {
        bool isOverlapping = Physics.CheckSphere(position, bulletCheckRadius, pillarLayer); // Check for overlapping pillars

        return !isOverlapping; // Return true if position is clear
    }

    // --- Draw gizmos to visualize spawn area in the editor --- \\
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius); // Draw spawn radius
        Gizmos.DrawCube(transform.position + new Vector3(0, spawnHeight, 0), Vector3.one * 0.2f); // Draw spawn height indicator
    }
}
