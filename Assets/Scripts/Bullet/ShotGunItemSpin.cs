using UnityEngine;

public class ShotGunItemSpin : MonoBehaviour
{
    private float moveSpeed = 1f;
    private float amplitude = 0.5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, 0, 1, Space.Self);

        float verticalMovement = Mathf.Sin(Time.time * moveSpeed) * amplitude;

        Vector3 newPos = startPos + Vector3.up * verticalMovement;
        transform.position = newPos;
    }
}
