using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime = 10f;
    public int damage = 1;
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

        }
        else if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
