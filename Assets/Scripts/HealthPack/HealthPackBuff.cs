using UnityEngine;

public class HealthPackBuff : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(0, 1, 0, Space.Self); // Rotate the health pack around its Y-axis
    }
}
