using UnityEngine;

public class BulletPickedUp : MonoBehaviour
{
    // === BULLET PICKED UP === \\
    // This script destroys the bullet object when it collides with another collider
    // (e.g., when picked up by the player).
    // ========================== \\

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
