using UnityEngine;

public class BulletSpin : MonoBehaviour
{
    // === BULLET SPIN SCRIPT === \\
    // This script makes the bullet spin around its Y-axis.
    // Attach this script to the bullet Prefab.
    // ============================ \\

    void Update()
    {
        transform.Rotate(0, 1, 0, Space.Self); // Rotate the bullet around its Y-axis
    }
}
