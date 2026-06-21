using UnityEngine;

using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 30;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " recibe " + damage + " de daño.");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
