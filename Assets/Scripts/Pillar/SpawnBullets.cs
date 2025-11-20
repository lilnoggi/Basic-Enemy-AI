using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    // === SPAWN BULLETS === \\
    // This script is intended to handle the spawning of bullet objects
    // around a pillar in the game.
    // ======================== \\

    public GameObject bulletPrefab;
    public int numToSpawn = 1;
    public float spawnRadius = 1f;
    public float spawnHeight = 1f;

    void Start()
    {
        SpawnNumOfBullets();
    }

    void SpawnNumOfBullets()
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomPoint2D = Random.insideUnitCircle * spawnRadius;

        Vector3 center = transform.position;

        Vector3 randomPosition = center + new Vector3(randomPoint2D.x, spawnHeight, randomPoint2D.y);

        return randomPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.DrawCube(transform.position + new Vector3(0, spawnHeight, 0), Vector3.one * 0.2f);
    }
}
