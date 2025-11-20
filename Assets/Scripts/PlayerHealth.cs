using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // === PLAYER HEALTH CONTROLLER SCRIPT === \\
    // This script manages the player's health.
    // It handles taking damage and eventually healing.
    // Attach this script to the player GameObject in the scene.
    // ========================================== \\

    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum health
    public float currentHealth; // Current health

    [Header("Health UI")]
    public TextMeshProUGUI healthText; // UI element to display health
    public GameObject gameOverPanel; // Game over panel

    [Header("Player Health Pack")]
    public GameObject healthPackPrefab;
    public float spawnRadius = 1.0f;
    private bool healthPackSpawned = false; 

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health
    }

    private void Update()
    {
        // Update the health UI text
        healthText.text = $"Health: {currentHealth}/{maxHealth}";

        // If player's health is less than or equal to 45, spawn a health pack.
        if (currentHealth <= 45 && !healthPackSpawned)
        {
            SpawnHealthPack();
            healthPackSpawned = true;
        }
    }

    // Method to apply damage to the player \\
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Reduce current health by damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and maxHealth
        if (currentHealth <= 0)
        {
            Die(); // Call Die method if health reaches zero
        }
    }

    // Method to handle player death \\
    private void Die()
    {
        // Enable mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop time
        Time.timeScale = 0f;

        // Handle player death (e.g., respawn, game over screen, etc.)
        Debug.Log("Player has died.");
        gameOverPanel.SetActive(true); // Show game over panel
    }

    // --- Spawn Health Pack --- \\
    public void SpawnHealthPack()
    {
        if (healthPackPrefab != null)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPos = new Vector3(
                transform.position.x + randomPoint.x,
                transform.position.y,
                transform.position.z + randomPoint.y
                );

            Instantiate(healthPackPrefab, spawnPos, Quaternion.identity );
        }
    }

    // --- Heal the player --- \\
    public void HealFull()
    {
        currentHealth = maxHealth;
        healthPackSpawned = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
