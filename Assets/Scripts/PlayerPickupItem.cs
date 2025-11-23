using TMPro;
using UnityEngine;

public class PlayerPickupItem : MonoBehaviour
{
    // === PLAYER PICKUP ITEM SCRIPT === \\
    // This script allows the player to pick up bullet items in the game.
    // It increments the player's ammo count and updates the UI accordingly.
    // Attach this script to the player Prefab.
    // ==================================== \\

    [Header("Bullet Item")]
    int bulletsCollected = 0;
    public TextMeshProUGUI ammoCount;
    public GameObject playerShotgun;

    [Header("Shotgun/Shooting")]
    bool hasShotgun = false;
    public float bulletSpeed = 10f;
    public float bulletDamage = 40f;
    public Transform bulletFirePoint;
    public GameObject bulletPrefab;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && hasShotgun
            && bulletsCollected >= 1)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        bulletsCollected--;
        UpdateUI();

        GameObject tempBullet = Instantiate(bulletPrefab, bulletFirePoint.position, bulletFirePoint.rotation);

        Bullet bulletScript = tempBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = bulletSpeed;
            bulletScript.damage = bulletDamage;
        }
    }

    public void UpdateUI()
    {
        ammoCount.text = $"Ammo: {bulletsCollected}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletItem"))
        {
            bulletsCollected++;
            UpdateUI();
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
            hasShotgun = true;
        }
    }
}
