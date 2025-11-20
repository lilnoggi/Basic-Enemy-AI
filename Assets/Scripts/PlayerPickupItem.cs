using TMPro;
using UnityEngine;

public class PlayerPickupItem : MonoBehaviour
{
    // === PLAYER PICKUP ITEM SCRIPT === \\
    // This script allows the player to pick up bullet items in the game.
    // It increments the player's ammo count and updates the UI accordingly.
    // Attach this script to the player Prefab.
    // ==================================== \\

    int bulletsCollected = 0;
    public TextMeshProUGUI ammoCount;
    public GameObject playerShotgun;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletItem"))
        {
            bulletsCollected++;
            ammoCount.text = "Ammo: " + bulletsCollected;
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("HealthPack"))
        {
            playerHealth.HealFull();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("ShotgunItem"))
        {
            playerShotgun.SetActive(true);
            Destroy(other.gameObject);
        }
    }
}
