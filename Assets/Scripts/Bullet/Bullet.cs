using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;

    void Start()
    {
        Destroy(gameObject, 5f); // Destroy game object after 5 seconds.
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        else if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }

            else if (other.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
