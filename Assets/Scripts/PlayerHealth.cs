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

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum health
    }

    private void Update()
    {
        // Update the health UI text
        healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }

}
