using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Damage Feedback")]
    [SerializeField] private float flashDelay = 1f;
    [SerializeField] private float flashTime = 0.12f;

    [Header("Death")]
    [SerializeField] private float destroyDelay = 1f;
    
    [Header("Drops")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinsToDrop = 0;
    [SerializeField] private float dropRadius = 0.6f;

    [SerializeField] private bool isBoss;
    private int currentHealth;
    private bool isDead;
    private Color originalColor;

    private void Awake()
    {
        currentHealth = maxHealth;
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        StartCoroutine(DamageFlashCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlashCoroutine()
    {
        yield return new WaitForSeconds(flashDelay);

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.color = originalColor;
    }

    private void DropCoins()
    {
        if (coinPrefab == null) return;

        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * dropRadius;

            Instantiate(
                coinPrefab,
                transform.position + randomOffset,
                Quaternion.identity);
        }
    }
    
    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.SetTrigger("Die");

        DropCoins();
        
        if (isBoss)
        {
            StartCoroutine(ShowVictoryAfterDelay());
        }
        else
        {
            Destroy(gameObject, destroyDelay);
        }
    }
    
    private IEnumerator ShowVictoryAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        VictoryMenu victoryMenu = FindFirstObjectByType<VictoryMenu>();

        victoryMenu.ShowVictory();

        Destroy(gameObject);
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
